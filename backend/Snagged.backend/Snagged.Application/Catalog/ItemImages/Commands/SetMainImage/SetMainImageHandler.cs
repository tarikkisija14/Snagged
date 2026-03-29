using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

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

            
            var siblings = await ctx.ItemImages
                .Where(x => x.ItemId == image.ItemId && x.Id != image.Id)
                .ToListAsync(ct);

            foreach (var img in siblings)
                img.IsMain = false;

            image.IsMain = true;

            await ctx.SaveChangesAsync(ct);
            return true;
        }
    }
}