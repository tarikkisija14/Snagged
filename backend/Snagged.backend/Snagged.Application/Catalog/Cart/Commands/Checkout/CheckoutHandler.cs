using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Payment;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Cart.Commands.Checkout
{
    public class CheckoutHandler(IAppDbContext ctx, ICurrentUserService currentUser, IWebPushService pushService)
        : IRequestHandler<CheckoutCommand, int>
    {
        public async Task<int> Handle(CheckoutCommand request, CancellationToken ct)
        {
            var userId = currentUser.UserId;

            var cart = await ctx.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsSavedForLater, ct);

            if (cart is null || !cart.CartItems.Any())
                throw new SnaggedNotFoundException("Cart is empty or does not exist.");

            var soldItems = cart.CartItems.Where(ci => ci.Item.IsSold).ToList();
            if (soldItems.Any())
            {
                var soldTitles = string.Join(", ", soldItems.Select(ci => ci.Item.Title));
                throw new SnaggedBusinessRuleException(
                    "ITEMS_UNAVAILABLE",
                    $"The following items are no longer available: {soldTitles}.");
            }

            var order = new Order
            {
                BuyerId = userId,
                OrderDate = DateTime.UtcNow,
                Status = PaymentStatus.Pending
            };

            foreach (var cartItem in cart.CartItems)
            {
                order.OrderItems.Add(new Snagged.Domain.Entities.OrderItem
                {
                    ItemId = cartItem.ItemId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Item.Price
                });
            }

            ctx.Orders.Add(order);
            ctx.CartItems.RemoveRange(cart.CartItems);
            await ctx.SaveChangesAsync(ct);

            
            ctx.Notifications.Add(new Notification
            {
                UserId = userId,
                Message = $"Your order #{order.Id} has been placed successfully! Proceed to payment.",
                NotificationType = "Order",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            await pushService.SendAsync(
                userId,
                "Order placed 🛍️",
                $"Your order #{order.Id} has been placed. Proceed to payment.",
                ct);

            
            var sellerIds = cart.CartItems
                .Select(ci => ci.Item.UserId)
                .Distinct();

            foreach (var sellerId in sellerIds)
            {
                ctx.Notifications.Add(new Notification
                {
                    UserId = sellerId,
                    Message = $"Someone placed an order containing your item(s)! Order #{order.Id}",
                    NotificationType = "Sale",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                await pushService.SendAsync(
                    sellerId,
                    "New order for your item 🛒",
                    $"Someone ordered your item(s). Order #{order.Id}",
                    ct);
            }

            await ctx.SaveChangesAsync(ct);

            return order.Id;
        }
    }
}