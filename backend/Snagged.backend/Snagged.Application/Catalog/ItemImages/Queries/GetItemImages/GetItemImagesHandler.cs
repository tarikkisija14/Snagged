using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.ItemImages.Queries.GetItemImages
{
    public class GetItemImagesHandler(IAppDbContext ctx) : IRequestHandler<GetItemImagesQuery, List<ItemImageDto>>
    {
        public async Task<List<ItemImageDto>> Handle(GetItemImagesQuery request, CancellationToken ct)
        {
            return await ctx.ItemImages
                .Where(i => i.ItemId == request.ItemId)
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.Id)
                .Select(i => new ItemImageDto
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMain
                })
                .ToListAsync(ct);
        }
    }
}