using MediatR;
using Snagged.Application.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Orders.Queries.GetOrdersById
{
    public class GetOrdersByIdQuery : IRequest<OrderDto>
    {
        public int Id { get; set; }
    }
}
