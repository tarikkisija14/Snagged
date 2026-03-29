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
                .Where(c => c.UserId == userId && c.IsSavedForLater)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    Items = c.CartItems.Select(ci => new CartItemDto
                    {
                        Id = ci.Id,
                        ItemId = ci.ItemId,
                        ItemName = ci.Item.Title,
                        ImageUrl = ci.Item.Images
                                      .Where(img => img.IsMain)
                                      .Select(img => img.ImageUrl)
                                      .FirstOrDefault()
                                   ?? ci.Item.Images
                                      .Select(img => img.ImageUrl)
                                      .FirstOrDefault(),
                        Price = ci.Item.Price,
                        Quantity = ci.Quantity,
                        AddedAt = ci.AddedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);

            return cart;
        }
    }
}