using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.DeleteCartItem
{
    public class DeleteCartItemHandler(IAppDbContext ctx) : IRequestHandler<DeleteCartItemCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteCartItemCommand request, CancellationToken ct)
        {
            var cartItem = await ctx.CartItems.FindAsync(new object[] { request.CartItemId }, ct);
            if (cartItem == null)
                throw new KeyNotFoundException($"CartItem with Id {request.CartItemId} not found.");

            ctx.CartItems.Remove(cartItem);
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
