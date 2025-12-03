using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.UpdateItemImage
{
    public class UpdateItemImageCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
