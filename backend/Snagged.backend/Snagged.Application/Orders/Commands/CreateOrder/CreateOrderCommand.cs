using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand :IRequest<int>
    {
        public required CreateOrderDto Order { get; set; }
    }
}
