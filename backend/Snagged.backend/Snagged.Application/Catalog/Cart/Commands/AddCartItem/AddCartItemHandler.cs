using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Cart.Commands.AddCartItem
{
    public class AddCartItemHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<AddCartItemCommand, int>
    {
        public async Task<int> Handle(AddCartItemCommand request, CancellationToken ct)
        {
            
            var userId = currentUser.UserId;

            var itemExists = await ctx.Items.AnyAsync(i => i.Id == request.ItemId && !i.IsSold, ct);
            if (!itemExists)
                throw new SnaggedNotFoundException(
                    $"Item with id {request.ItemId} was not found or is already sold.");

            var cart = await ctx.Carts
                .Include(c => c.CartItems)
               
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsSavedForLater, ct);

            if (cart is null)
            {
                cart = new Snagged.Domain.Entities.Cart { UserId = userId };
                ctx.Carts.Add(cart);
                await ctx.SaveChangesAsync(ct);
            }

            var existingItem = cart.CartItems.FirstOrDefault(i => i.ItemId == request.ItemId);
            if (existingItem is not null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                ctx.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity
                });
            }

            await ctx.SaveChangesAsync(ct);
            return cart.Id;
        }
    }
}