using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Orders.Commands
{
    public class CreateOrderItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
    }
}
