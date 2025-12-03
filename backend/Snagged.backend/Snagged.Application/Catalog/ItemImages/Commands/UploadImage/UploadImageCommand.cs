using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.UploadImage
{
    public class UploadImageCommand : IRequest<List<ItemImageDto>>
    {
        public int ItemId { get; set; }
        public List<byte[]> FilesBytes { get; set; } = new List<byte[]>();
        public List<string> FileNames { get; set; } = new List<string>();
    }
}
