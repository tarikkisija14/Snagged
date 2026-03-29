using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Dto;

namespace Snagged.Application.Catalog.Items.Queries.GetItems
{
    public class GetItemsQueryHandler(IAppDbContext ctx)
        : IRequestHandler<GetItemsQuery, List<ItemDto>>
    {
        public async Task<List<ItemDto>> Handle(GetItemsQuery request, CancellationToken ct)
        {
            var query = ctx.Items
                .AsNoTracking()
                .IncludeItemDetails();

            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(i => i.Title.Contains(request.Search));

            return await query
                .OrderByDescending(i => i.CreatedAt)
                .ProjectToDto()
                .ToListAsync(ct);
        }
    }
}