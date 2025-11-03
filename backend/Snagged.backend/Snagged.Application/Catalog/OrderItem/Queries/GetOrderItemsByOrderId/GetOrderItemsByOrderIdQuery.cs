using MediatR;
using Snagged.Application.Catalog.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Queries.GetOrderItemsByOrderId
{
    public class GetOrderItemsByOrderIdQuery:IRequest<List<OrderItemDto>>
    {
        public int OrderId { get; set; }
    }
}
