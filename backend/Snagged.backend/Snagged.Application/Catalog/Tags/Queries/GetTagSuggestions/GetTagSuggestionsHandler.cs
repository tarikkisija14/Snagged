using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Tags.Queries.GetTagSuggestions
{
    public class GetTagSuggestionsHandler(IAppDbContext ctx)
        : IRequestHandler<GetTagSuggestionsQuery, List<TagSuggestionDto>>
    {
        public async Task<List<TagSuggestionDto>> Handle(
            GetTagSuggestionsQuery request, CancellationToken ct)
        {
            var query = ctx.Tags.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var normalised = request.Query.Trim().ToLowerInvariant();
                query = query.Where(t => t.Name.Contains(normalised));
            }

            return await query
                .OrderBy(t => t.Name)
                .Take(request.Limit)
                .Select(t => new TagSuggestionDto { Id = t.Id, Name = t.Name })
                .ToListAsync(ct);
        }
    }
}