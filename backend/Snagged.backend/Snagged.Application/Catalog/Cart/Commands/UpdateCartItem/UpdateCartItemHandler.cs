using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Cart.Commands.UpdateCartItem
{
    public class UpdateCartItemHandler(IAppDbContext ctx)
        : IRequestHandler<UpdateCartItemCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCartItemCommand request, CancellationToken ct)
        {
            var cartItem = await ctx.CartItems.FindAsync(new object[] { request.CartItemId }, ct);

            if (cartItem is null)
                throw new SnaggedNotFoundException($"Cart item with id {request.CartItemId} was not found.");

            cartItem.Quantity = request.Quantity;
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}