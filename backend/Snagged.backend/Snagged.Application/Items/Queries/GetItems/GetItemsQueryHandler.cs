using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Items.Queries.GetItems
{
    public sealed class GetItemsQueryHandler(IAppDbContext ctx)
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
                    CategoryName = i.Category.Name,
                    SubcategoryName = i.Subcategory != null ? i.Subcategory.Name : null,
                    SellerUsername = i.User.Profile != null ? i.User.Profile.Username : i.User.Email,
                    ImageUrls = i.Images.Select(img => img.ImageUrl).ToList()
                });

            return await projectedQuery.ToListAsync(ct);
        }
    }
}
