using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Cart.Queries.GetSavedCartByUser
{
    public class GetSavedCartByUserHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<GetSavedCartByUserQuery, CartDto?>
    {
        public async Task<CartDto?> Handle(GetSavedCartByUserQuery request, CancellationToken ct)
        {
            var userId = currentUser.UserId;

            var cart = await ctx.Carts
                .AsNoTracking()
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                        .ThenInclude(i => i.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsSavedForLater, ct);

            if (cart is null)
                return null;

            return cart.ToDto();
        }
    }
}