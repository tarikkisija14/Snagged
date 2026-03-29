using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Cart.Queries.GetAllCarts
{
    public class GetAllCartsHandler(IAppDbContext ctx) : IRequestHandler<GetAllCartsQuery, List<CartDto>>
    {
        public async Task<List<CartDto>> Handle(GetAllCartsQuery request, CancellationToken ct)
        {
            return await ctx.Carts
                .AsNoTracking()
                .Where(c => !c.IsSavedForLater)
                .Select(cart => new CartDto
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    CreatedAt = cart.CreatedAt,
                    UpdatedAt = cart.UpdatedAt,
                    Items = cart.CartItems.Select(ci => new CartItemDto
                    {
                        Id = ci.Id,
                        ItemId = ci.ItemId,
                        ItemName = ci.Item.Title,
                        Quantity = ci.Quantity,
                        AddedAt = ci.AddedAt,
                        Price = ci.Item.Price
                    }).ToList()
                })
                .ToListAsync(ct);
        }
    }
}