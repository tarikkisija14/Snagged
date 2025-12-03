using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Queries.GetAllImages
{
    public class GetAllImagesHandler(IAppDbContext ctx) : IRequestHandler<GetAllImagesQuery, List<ItemImageDto>>
    {
        public async Task<List<ItemImageDto>> Handle(GetAllImagesQuery request, CancellationToken ct)
        {
            return await ctx.ItemImages
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
