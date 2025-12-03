using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.AddItemImage
{
    public class AddItemImageCommand:IRequest<int>
    {
        public int ItemId { get; set; }
        public string ImageUrl { get; set; }
    }
}
