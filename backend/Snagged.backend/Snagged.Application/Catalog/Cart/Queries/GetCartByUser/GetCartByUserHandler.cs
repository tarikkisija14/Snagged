using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Cart.Queries.GetCartByUser
{
    public class GetCartByUserHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<GetCartByUserQuery, CartDto?>
    {
        public async Task<CartDto?> Handle(GetCartByUserQuery request, CancellationToken ct)
        {
            var userId = currentUser.UserId;

            var cart = await ctx.Carts
                .AsNoTracking()
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                        .ThenInclude(i => i.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsSavedForLater, ct);

            return cart?.ToDto(isSavedForLater: false);
        }
    }
}