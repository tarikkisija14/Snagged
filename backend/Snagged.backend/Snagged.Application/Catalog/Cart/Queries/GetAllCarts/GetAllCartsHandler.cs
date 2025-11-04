using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Queries.GetAllCarts
{
    public class GetAllCartsHandler(IAppDbContext ctx) : IRequestHandler<GetAllCartsQuery, List<CartDto>>
    {
        public async Task<List<CartDto>> Handle(GetAllCartsQuery request, CancellationToken ct)
        {
            var carts = await ctx.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Item)
                .ToListAsync(ct);

            return carts.Select(cart => new CartDto
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
                    AddedAt = ci.AddedAt
                }).ToList()
            }).ToList();
        }
    }
}
