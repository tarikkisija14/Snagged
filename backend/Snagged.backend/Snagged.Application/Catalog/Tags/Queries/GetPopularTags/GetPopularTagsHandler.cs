using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Tags.Queries.GetPopularTags
{
    public class GetPopularTagsHandler(IAppDbContext ctx)
        : IRequestHandler<GetPopularTagsQuery, List<PopularTagDto>>
    {
        public async Task<List<PopularTagDto>> Handle(
            GetPopularTagsQuery request, CancellationToken ct)
        {
            return await ctx.Tags
                .AsNoTracking()
                .Where(t => t.ItemTags.Any())
                .OrderByDescending(t => t.ItemTags.Count)
                .Take(request.Limit)
                .Select(t => new PopularTagDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    ItemCount = t.ItemTags.Count
                })
                .ToListAsync(ct);
        }
    }
}