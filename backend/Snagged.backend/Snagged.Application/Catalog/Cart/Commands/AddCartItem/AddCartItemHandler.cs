using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.AddCartItem
{
    public class AddCartItemHandler(IAppDbContext ctx) : IRequestHandler<AddCartItemCommand, int>
    {
        public async Task<int> Handle(AddCartItemCommand request, CancellationToken ct)
        {
            var cart = await ctx.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, ct);

            if (cart == null)
            {
                cart = new Snagged.Domain.Entities.Cart  { UserId = request.UserId };
                ctx.Carts.Add(cart);
                await ctx.SaveChangesAsync(ct);
            }

            var existingItem = cart.CartItems.FirstOrDefault(i => i.ItemId == request.ItemId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity
                };
                ctx.CartItems.Add(cartItem);
            }

            await ctx.SaveChangesAsync(ct);
            return cart.Id;

        }
    }
}
