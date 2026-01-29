using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Dto;  
using Snagged.Application.Catalog.Items;     

namespace Snagged.Application.Catalog.Items.Queries.GetItems
{
    public class GetItemsQueryHandler(IAppDbContext ctx)
        : IRequestHandler<GetItemsQuery, List<ItemDto>>
    {
        public async Task<List<ItemDto>> Handle(GetItemsQuery request, CancellationToken ct)
        {
            var q = ctx.Items
                .Include(i => i.Category)
                .Include(i => i.Subcategory)
                .Include(i => i.User)
                    .ThenInclude(u => u.Profile)
                .Include(i => i.Images)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
                q = q.Where(i => i.Title.Contains(request.Search));

            var projectedQuery = q.OrderByDescending(i => i.CreatedAt)
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
                    Images = i.Images.Select(img => new ItemImageDto
                    {
                        Id = img.Id,
                        ImageUrl = img.ImageUrl,
                        IsMain = img.IsMain
                    }).ToList()
                });

            return await projectedQuery.ToListAsync(ct);
        }
    }
}