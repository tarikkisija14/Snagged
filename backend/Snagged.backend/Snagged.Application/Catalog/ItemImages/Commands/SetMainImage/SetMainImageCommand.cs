using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.SetMainImage
{
    public class SetMainImageCommand:IRequest<bool>
    {
        public int ImageId { get; set; }
    }
}
