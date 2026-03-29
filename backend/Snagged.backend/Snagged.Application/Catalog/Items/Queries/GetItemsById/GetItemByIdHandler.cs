using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Items.Queries.GetItemsById
{
    public class GetItemByIdHandler(IAppDbContext ctx) : IRequestHandler<GetItemByIdQuery, ItemDto>
    {
        public async Task<ItemDto> Handle(GetItemByIdQuery query, CancellationToken ct)
        {
            var item = await ctx.Items
                .AsNoTracking()
                .IncludeItemDetails()
                .ProjectToDto()
                .FirstOrDefaultAsync(i => i.Id == query.Id, ct);

            if (item is null)
                throw new SnaggedNotFoundException($"Item with id {query.Id} was not found.");

            return item;
        }
    }
}