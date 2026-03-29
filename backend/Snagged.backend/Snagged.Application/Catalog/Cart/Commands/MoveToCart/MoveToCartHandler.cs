using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Cart.Commands.MoveToCart
{
    public class MoveToCartHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<MoveToCartCommand, Unit>
    {
        public async Task<Unit> Handle(MoveToCartCommand request, CancellationToken ct)
        {
            var cart = await ctx.Carts
                .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

            if (cart is null)
                throw new SnaggedNotFoundException($"Cart with id {request.CartId} was not found.");

            if (cart.UserId != currentUser.UserId)
                throw new UnauthorizedAccessException();

            cart.IsSavedForLater = false;
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}