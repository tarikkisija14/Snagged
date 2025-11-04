using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem
{
    public class UpdateOrderItemDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
