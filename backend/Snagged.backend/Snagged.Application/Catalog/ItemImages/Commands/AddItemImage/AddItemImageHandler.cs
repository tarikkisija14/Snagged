using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;

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
            await ctx.SaveChangesAsync(ct);

            return img.Id;
        }
    }
}