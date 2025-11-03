using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Commands.CreateOrderItem
{
    public class CreateOrderItemCommand :IRequest<int>
    {
        public int OrderId {  get; set; }
        public CreateOrderItemDto Item { get; set; } = new();
    }
}
