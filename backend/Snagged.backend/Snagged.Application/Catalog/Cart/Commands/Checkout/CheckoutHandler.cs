using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.Checkout
{
    public class CheckoutHandler(IAppDbContext ctx) : IRequestHandler<CheckoutCommand, int>
    {
        public async Task<int> Handle(CheckoutCommand request, CancellationToken ct)
        {
            var cart = await ctx.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, ct);

            if (cart == null || !cart.CartItems.Any())
                throw new KeyNotFoundException("Cart is empty.");

            var order = new Order
            {
                BuyerId = request.UserId,
                OrderDate = DateTime.Now,
                Status = "Pending"
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
            if (request.AddressId.HasValue)
            {
                var address = await ctx.Addresses.FindAsync(new object[] { request.AddressId.Value }, ct);
                if (address != null)
                {
                    
                }
            }

            ctx.CartItems.RemoveRange(cart.CartItems);

            await ctx.SaveChangesAsync(ct);

            return order.Id;

        }
    }
}
