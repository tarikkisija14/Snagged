using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Items.Queries.GetItemSuggestions
{
    public class GetItemSuggestionsHandler(IAppDbContext ctx)
        : IRequestHandler<GetItemSuggestionsQuery, List<ItemSuggestionDto>>
    {
        public async Task<List<ItemSuggestionDto>> Handle(
            GetItemSuggestionsQuery request, CancellationToken ct)
        {
            var term = request.Query.Trim();
            var limit = Math.Clamp(request.Limit, 1, 20);

            return await ctx.Items
                .AsNoTracking()
                .Where(i => !i.IsSold && i.Title.Contains(term))
                .OrderBy(i => i.Title)
                .Take(limit)
                .Select(i => new ItemSuggestionDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Price = i.Price,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.Images
                                     .OrderByDescending(img => img.IsMain)
                                     .Select(img => img.ImageUrl)
                                     .FirstOrDefault()
                })
                .ToListAsync(ct);
        }
    }
}