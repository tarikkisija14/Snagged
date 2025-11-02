using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Commom.Paging;
using Snagged.Application.Items.Queries.GetItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Items.Queries.GetPagedItems
{
    public class GetPagedItemsHandler(IAppDbContext ctx) : IRequestHandler<GetPagedItemsQuery, PageResult<ItemDto>>
    {
        public async Task<PageResult<ItemDto>> Handle(GetPagedItemsQuery request, CancellationToken ct)
        {
            var query = ctx.Items
                .Include(i => i.Category)
                .Include(i => i.Subcategory)
                .Include(i => i.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
                query = query.Where(i => i.Title.Contains(request.Title));

            if (request.CategoryId.HasValue)
                query = query.Where(i => i.CategoryId == request.CategoryId.Value);

            if (request.SubcategoryId.HasValue)
                query = query.Where(i => i.SubcategoryId == request.SubcategoryId.Value);

            if (request.IsSold.HasValue)
                query = query.Where(i => i.IsSold == request.IsSold.Value);

            var projectedQuery = query.Select(i => new ItemDto
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

            return await PageResult<ItemDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
