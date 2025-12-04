using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Queries.GetCartByUser
{
    public class GetCartByUserHandler(IAppDbContext ctx) : IRequestHandler<GetCartByUserQuery, CartDto>
    {
        public async Task<CartDto> Handle(GetCartByUserQuery request, CancellationToken ct)
        {
            var cart = await ctx.Carts
                 .Include(c => c.CartItems)
                     .ThenInclude(ci => ci.Item)
                 .FirstOrDefaultAsync(c => c.UserId == request.UserId && !c.IsSavedForLater, ct);

            if (cart == null)
                return null;

            return new CartDto
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
            };
        }
    }
}
