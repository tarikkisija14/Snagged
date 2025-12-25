using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Queries.GetItems;
using Snagged.Application.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Queries.GetPagedItems
{
    public class GetPagedItemsHandler(IAppDbContext ctx) : IRequestHandler<GetPagedItemsQuery, PageResult<ItemDto>>
    {
        public async Task<PageResult<ItemDto>> Handle(GetPagedItemsQuery request, CancellationToken ct)
        {
            var query = ctx.Items
            .Include(i => i.Category)
            .Include(i => i.Subcategory)
            .Include(i => i.User)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new ItemDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Price = i.Price,
                Condition = i.Condition,
                IsSold = i.IsSold,
                CreatedAt = i.CreatedAt,
                CategoryName = i.Category.Name,
                SubcategoryName = i.Subcategory != null ? i.Subcategory.Name : null,
                SellerUsername = i.User.Email,
                ImageUrls = i.Images.Select(img => img.ImageUrl).ToList()
            });

            return await PageResult<ItemDto>.FromQueryableAsync(query, request.Paging, ct);
        }
    }
}
