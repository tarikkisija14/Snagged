using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Application.Common.Paging;

namespace Snagged.Application.Catalog.Items.Queries.GetPagedItems
{
    public class GetPagedItemsHandler(IAppDbContext ctx) : IRequestHandler<GetPagedItemsQuery, PageResult<ItemDto>>
    {
        public async Task<PageResult<ItemDto>> Handle(GetPagedItemsQuery request, CancellationToken ct)
        {
            var query = ctx.Items
                .AsNoTracking()
                .IncludeItemDetails()
                .OrderByDescending(i => i.CreatedAt)
                .ProjectToDto();

            return await PageResult<ItemDto>.FromQueryableAsync(query, request.Paging, ct);
        }
    }
}