using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Cart.Commands.DeleteCartItem
{
    public class DeleteCartItemHandler(IAppDbContext ctx) : IRequestHandler<DeleteCartItemCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCartItemCommand request, CancellationToken ct)
        {
            var cartItem = await ctx.CartItems.FindAsync(new object[] { request.CartItemId }, ct);

            if (cartItem is null)
                throw new SnaggedNotFoundException($"Cart item with id {request.CartItemId} was not found.");

            ctx.CartItems.Remove(cartItem);
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}