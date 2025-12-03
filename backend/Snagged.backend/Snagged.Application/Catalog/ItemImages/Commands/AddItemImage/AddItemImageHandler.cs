using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.AddItemImage
{
    public class AddItemImageHandler(IAppDbContext ctx) : IRequestHandler<AddItemImageCommand, int>
    {
        public async Task<int> Handle(AddItemImageCommand request, CancellationToken ct)
        {
            var img = new ItemImage
            {
                ItemId = request.ItemId,
                ImageUrl = request.ImageUrl
            };

            ctx.ItemImages.Add(img);
            await ctx.SaveChangesAsync();

            return img.Id;
        }
    }
}
