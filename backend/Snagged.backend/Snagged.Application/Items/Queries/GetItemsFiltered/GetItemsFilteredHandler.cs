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

namespace Snagged.Application.Items.Queries.GetItemsFiltered
{
    public class GetItemsFilteredHandler(IAppDbContext ctx) : IRequestHandler<GetItemsFilteredQuery, PageResult<ItemDto>>
    {
        public async Task<PageResult<ItemDto>> Handle(GetItemsFilteredQuery request, CancellationToken ct)
        {
            var query = ctx.Items
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Subcategory)
                .Include(x => x.Images)
                .AsQueryable();


            if (request.UserId.HasValue)
                query = query.Where(x => x.UserId == request.UserId.Value);

            if (request.CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == request.CategoryId.Value);

            if (request.SubcategoryId.HasValue)
                query = query.Where(x => x.SubcategoryId == request.SubcategoryId.Value);

            if (!string.IsNullOrWhiteSpace(request.TitleContains))
                query = query.Where(x => x.Title.Contains(request.TitleContains));

            if (!string.IsNullOrWhiteSpace(request.Condition))
                query = query.Where(x => x.Condition == request.Condition);

            if (request.IsSold.HasValue)
                query = query.Where(x => x.IsSold == request.IsSold.Value);

            var dtoQuery = query.Select(x => new ItemDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                Condition = x.Condition,
                IsSold = x.IsSold,
                CategoryName = x.Category.Name,
                SubcategoryName = x.Subcategory != null ? x.Subcategory.Name : null,
                SellerUsername = x.User.Email,
                ImageUrls = x.Images.Select(i => i.ImageUrl).ToList()
            });

            return await PageResult<ItemDto>.FromQueryableAsync(dtoQuery, request.Paging, ct);
        }
    }
}
