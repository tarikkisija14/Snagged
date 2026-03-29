using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.ItemImages;
using Snagged.Application.Common.Exceptions;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.ItemImages.Commands.UploadImage
{
    public class UploadImageHandler : IRequestHandler<UploadImageCommand, List<ItemImageDto>>
    {
        private readonly IAppDbContext _ctx;
        private readonly string _itemsFolder;

        public UploadImageHandler(IAppDbContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _itemsFolder = Path.Combine(env.ContentRootPath, "images", "items");

            if (!Directory.Exists(_itemsFolder))
                Directory.CreateDirectory(_itemsFolder);
        }

        public async Task<List<ItemImageDto>> Handle(UploadImageCommand request, CancellationToken ct)
        {
            var itemExists = await _ctx.Items.AnyAsync(x => x.Id == request.ItemId, ct);
            if (!itemExists)
                throw new SnaggedNotFoundException($"Item with id {request.ItemId} was not found.");

            var newImages = new List<ItemImage>();

            for (int i = 0; i < request.FilesBytes.Count; i++)
            {
                var extension = Path.GetExtension(request.FileNames[i]);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_itemsFolder, fileName);

                
                await File.WriteAllBytesAsync(filePath, request.FilesBytes[i], ct);

                var img = new ItemImage
                {
                    ItemId = request.ItemId,
                    ImageUrl = $"/images/items/{fileName}"
                };

                _ctx.ItemImages.Add(img);
                newImages.Add(img);
            }

            
            await _ctx.SaveChangesAsync(ct);

            return newImages.Select(img => new ItemImageDto
            {
                Id = img.Id,
                ItemId = img.ItemId,
                ImageUrl = img.ImageUrl
            }).ToList();
        }
    }
}