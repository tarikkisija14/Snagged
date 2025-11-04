using MediatR;
using Snagged.Application.Catalog.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Queries.GetOrdersById
{
    public class GetOrdersByIdQuery : IRequest<OrderDto>
    {
        public int Id { get; set; }
    }
}
