using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Cart.Commands.ClearCart
{
    public class ClearCartHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<ClearCartCommand, Unit>
    {
        public async Task<Unit> Handle(ClearCartCommand request, CancellationToken ct)
        {
           
            var userId = currentUser.UserId;

            var cart = await ctx.Carts
                .Include(c => c.CartItems)
                
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsSavedForLater, ct);

            if (cart is not null)
            {
                ctx.CartItems.RemoveRange(cart.CartItems);
                await ctx.SaveChangesAsync(ct);
            }

            return Unit.Value;
        }
    }
}