using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.SetMainImage
{
    public class SetMainImageHandler(IAppDbContext ctx) : IRequestHandler<SetMainImageCommand, bool>
    {
        public async Task<bool> Handle(SetMainImageCommand request, CancellationToken ct)
        {
            var image = await ctx.ItemImages
                .FirstOrDefaultAsync(i => i.Id == request.ImageId, ct);

            if (image == null)
                return false;

            
            var others = ctx.ItemImages
                .Where(x => x.ItemId == image.ItemId && x.Id != image.Id);

            foreach (var img in others)
                img.IsMain = false;

            
            image.IsMain = true;

            await ctx.SaveChangesAsync();
            return true;
        }
    }
}
