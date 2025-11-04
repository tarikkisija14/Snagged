using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.UpdateCartItem
{
    public class UpdateCartItemHandler(IAppDbContext ctx) : IRequestHandler<UpdateCartitemCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCartitemCommand request, CancellationToken ct)
        {
            var cartItem = await ctx.CartItems.FindAsync(new object[] { request.CartItemId }, ct);
            if (cartItem == null)
                throw new KeyNotFoundException($"CartItem with Id {request.CartItemId} not found.");

            cartItem.Quantity = request.Quantity;
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
