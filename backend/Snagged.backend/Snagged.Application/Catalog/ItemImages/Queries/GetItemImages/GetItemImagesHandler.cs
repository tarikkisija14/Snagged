using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Queries.GetItemImages
{
    public class GetItemImagesHandler(IAppDbContext ctx) : IRequestHandler<GetItemImagesQuery, List<ItemImageDto>>
    {
        public async Task<List<ItemImageDto>> Handle(GetItemImagesQuery request, CancellationToken ct)
        {
            return await ctx.ItemImages
                .Where(i => i.ItemId == request.ItemId)
                .Select(i => new ItemImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    ItemId = i.ItemId
                })
                .ToListAsync(ct);
        }
    }
}
