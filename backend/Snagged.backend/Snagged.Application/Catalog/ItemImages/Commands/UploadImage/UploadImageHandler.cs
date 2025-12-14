using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.UploadImage
{
    public class UploadImageHandler : IRequestHandler<UploadImageCommand, List<ItemImageDto>>
    {

        private readonly IAppDbContext _ctx;
        private readonly string _itemsFolder;

        public UploadImageHandler(IAppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;

            
            var projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            _itemsFolder = Path.Combine(projectRoot, config["ImageSettings:ItemsPath"]);

            if (!Directory.Exists(_itemsFolder))
                Directory.CreateDirectory(_itemsFolder);
        }

        public async Task<List<ItemImageDto>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var result = new List<ItemImageDto>();
            var itemExists = await _ctx.Items.AnyAsync(x => x.Id == request.ItemId, cancellationToken);
            if (!itemExists)
                throw new Exception($"Item with Id {request.ItemId} does not exist. Cannot upload image.");

            for (int i = 0; i < request.FilesBytes.Count; i++)
            {
                string extension = Path.GetExtension(request.FileNames[i]);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(_itemsFolder, fileName);

                try
                {
                    await File.WriteAllBytesAsync(filePath, request.FilesBytes[i], cancellationToken);

                    var img = new ItemImage
                    {
                        ItemId = request.ItemId,
                        ImageUrl = $"/images/items/{fileName}"  
                    };

                    _ctx.ItemImages.Add(img);
                    await _ctx.SaveChangesAsync(cancellationToken);

                    result.Add(new ItemImageDto
                    {
                        Id = img.Id,
                        ItemId = img.ItemId,
                        ImageUrl = img.ImageUrl
                    });
                }
                catch (Exception ex)
                {
                    
                    throw new Exception($"Error saving file {fileName}: {ex.Message} | Inner: {ex.InnerException?.Message}");
                }
            }

            return result;
        }

    }

    
}
