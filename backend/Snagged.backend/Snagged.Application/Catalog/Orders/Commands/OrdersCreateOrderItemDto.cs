using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Commands
{
    public class OrdersCreateOrderItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
    }
}
