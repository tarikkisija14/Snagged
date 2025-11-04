using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommand:IRequest<Unit>
    {
        public int Id { get; set; }
        public UpdateOrderItemDto Item { get; set; } = new();
    }
}
