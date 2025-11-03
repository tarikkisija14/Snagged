using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Orders.Commands
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
