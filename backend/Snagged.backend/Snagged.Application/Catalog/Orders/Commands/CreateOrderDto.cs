using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Commands
{
    public class CreateOrderDto
    {
        public int BuyerId { get; set; }
        public string Status { get; set; }
        public List<OrdersCreateOrderItemDto> Items { get; set; } = new();
    }
}
