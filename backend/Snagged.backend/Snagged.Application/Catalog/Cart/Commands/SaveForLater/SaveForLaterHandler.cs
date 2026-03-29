using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Cart.Commands.SaveForLater
{
    public class SaveForLaterHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<SaveForLaterCommand, Unit>
    {
        public async Task<Unit> Handle(SaveForLaterCommand request, CancellationToken ct)
        {
            var cart = await ctx.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

            if (cart is null)
                throw new SnaggedNotFoundException($"Cart with id {request.CartId} was not found.");

            if (cart.UserId != currentUser.UserId)
                throw new UnauthorizedAccessException();

            cart.IsSavedForLater = true;
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}