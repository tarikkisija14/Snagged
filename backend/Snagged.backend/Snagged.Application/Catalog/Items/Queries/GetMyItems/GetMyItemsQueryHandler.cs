using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Items.Queries.GetMyItems
{
    public class GetMyItemsQueryHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<GetMyItemsQuery, List<ItemDto>>
    {
        public async Task<List<ItemDto>> Handle(GetMyItemsQuery request, CancellationToken ct)
        {
            return await ctx.Items
                .AsNoTracking()
                .Where(i => i.UserId == currentUser.UserId)
                .IncludeItemDetails()
                .OrderByDescending(i => i.CreatedAt)
                .ProjectToDto()
                .ToListAsync(ct);
        }
    }
}