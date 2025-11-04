using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand:IRequest<Unit>
    {
        public int Id { get; set; }
        public required string Status { get; set; }
    }
}
