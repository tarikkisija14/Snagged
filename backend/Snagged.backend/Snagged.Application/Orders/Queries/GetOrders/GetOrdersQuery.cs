using MediatR;
using Snagged.Application.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Orders.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<List<OrderDto>>
    {
        public int? BuyerId { get; set; }
        public string? Status { get; set; }
    }
}
