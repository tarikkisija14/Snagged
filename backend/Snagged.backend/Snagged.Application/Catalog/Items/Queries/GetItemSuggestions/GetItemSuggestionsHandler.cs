using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Items.Queries.GetItemSuggestions
{
    public class GetItemSuggestionsHandler(IAppDbContext ctx)
        : IRequestHandler<GetItemSuggestionsQuery, List<string>>
    {
        public async Task<List<string>> Handle(GetItemSuggestionsQuery request, CancellationToken ct)
        {
            return await ctx.Items
                .AsNoTracking()
                .Where(i => i.Title.Contains(request.Query) && !i.IsSold)
                .Select(i => i.Title)
                .Take(8)
                .ToListAsync(ct);
        }
    }
}