using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Queries.GetItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Queries.GetItemsById
{
    public class GetItemByIdHandler(IAppDbContext ctx) :IRequestHandler<GetItemByIdQuery,ItemDto>
    {
        public async Task<ItemDto> Handle(GetItemByIdQuery query,CancellationToken ct)
        {
            var item = await ctx.Items.Include(i => i.Category)
                .Include(i => i.Subcategory)
                .Include(i => i.User).ThenInclude(u => u.Profile)
                .Include(i => i.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == query.Id, ct);


            if (item == null)
                return null;


            return new ItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Price = item.Price,
                Condition = item.Condition,
                IsSold = item.IsSold,
                CreatedAt = item.CreatedAt,
                CategoryName = item.Category.Name,
                SubcategoryName = item.Subcategory != null ? item.Subcategory.Name : null,
                SellerUsername = item.User.Profile != null ? item.User.Profile.Username : item.User.Email,
                ImageUrls = item.Images.Select(img => img.ImageUrl).ToList()
            };

        }
    }
}
