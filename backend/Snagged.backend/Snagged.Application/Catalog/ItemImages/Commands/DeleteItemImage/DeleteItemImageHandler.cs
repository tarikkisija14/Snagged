using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.DeleteItemImage
{
   public class DeleteItemImageHandler(IAppDbContext ctx) : IRequestHandler<DeleteItemImageCommand, bool>
    {
        public async Task<bool> Handle(DeleteItemImageCommand request, CancellationToken ct)
        {
            var img = await ctx.ItemImages
                .FirstOrDefaultAsync(i => i.Id == request.Id, ct);

            if (img == null)
                return false;

            ctx.ItemImages.Remove(img);
            await ctx.SaveChangesAsync();

            return true;
        }
    }
}
