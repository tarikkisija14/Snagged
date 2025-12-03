using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.UpdateItemImage
{
    public class UpdateItemImageHandler(IAppDbContext ctx) : IRequestHandler<UpdateItemImageCommand, bool>
    {
        public async Task<bool> Handle(UpdateItemImageCommand request, CancellationToken ct)
        {
            var image = await ctx.ItemImages
                .FirstOrDefaultAsync(i => i.Id == request.Id, ct);

            if (image == null)
                return false;

            image.ImageUrl = request.ImageUrl;

            await ctx.SaveChangesAsync();
            return true;
        }
    }
}
