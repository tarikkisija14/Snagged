using MediatR;
using Snagged.Application.Catalog.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand :IRequest<int>
    {
        public required CreateOrderDto Order { get; set; }
    }
}
