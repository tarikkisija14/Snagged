using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.DeleteItemImage
{
    public class DeleteItemImageCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
