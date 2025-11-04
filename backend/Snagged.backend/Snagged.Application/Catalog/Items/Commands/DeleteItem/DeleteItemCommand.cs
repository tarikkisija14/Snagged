using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Commands.DeleteItem
{
    public class DeleteItemCommand :IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
