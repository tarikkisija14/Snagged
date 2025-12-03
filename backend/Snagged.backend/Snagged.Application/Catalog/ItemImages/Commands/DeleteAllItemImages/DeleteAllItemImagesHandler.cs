using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.DeleteAllItemImages
{
    public class DeleteAllItemImagesHandler(IAppDbContext ctx) : IRequestHandler<DeleteAllItemImagesCommand, int>
    {
        public async Task<int> Handle(DeleteAllItemImagesCommand request, CancellationToken ct)
        {
            var images = await ctx.ItemImages
               .Where(i => i.ItemId == request.ItemId)
               .ToListAsync(ct);

            if (images.Count == 0)
                return 0;

            ctx.ItemImages.RemoveRange(images);
            await ctx.SaveChangesAsync();

            return images.Count;
        }
    }
}
