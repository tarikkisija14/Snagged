using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Cart.Commands.UpdateCartItem
{
    public class UpdateCartItemHandler(IAppDbContext ctx, ICurrentUserService currentUser)
    : IRequestHandler<UpdateCartitemCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCartitemCommand request, CancellationToken ct)
        {
            var cartItem = await ctx.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == request.CartItemId, ct);

            if (cartItem is null)
                throw new SnaggedNotFoundException($"Cart item with id {request.CartItemId} was not found.");

            if (cartItem.Cart.UserId != currentUser.UserId)
                throw new UnauthorizedAccessException("You do not own this cart item.");

            cartItem.Quantity = request.Quantity;
            await ctx.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}