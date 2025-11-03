using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Orders.Commands
{
    public class CreateOrderDto
    {
        public int BuyerId { get; set; }
        public string Status { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
