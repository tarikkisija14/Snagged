using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Application.Catalog.Items;
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
                    .ThenInclude(u => u.Profile)
                .Include(i => i.Images)     
                .Include(i => i.Favorites)      
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
                    CategoryId = i.CategoryId,
                    SubcategoryId = i.SubcategoryId,
                    LikesCount = i.Favorites.Count,
                    CategoryName = i.Category.Name,
                    SubcategoryName = i.Subcategory != null ? i.Subcategory.Name : null,
                    SellerUsername = i.User.Profile != null ? i.User.Profile.Username : i.User.Email,
                    Images = i.Images.Select(img => new ItemImageDto  
                    {
                        Id = img.Id,
                        ImageUrl = img.ImageUrl,
                        IsMain = img.IsMain
                    }).ToList()
                });

            return await PageResult<ItemDto>.FromQueryableAsync(query, request.Paging, ct);
        }
    }
}