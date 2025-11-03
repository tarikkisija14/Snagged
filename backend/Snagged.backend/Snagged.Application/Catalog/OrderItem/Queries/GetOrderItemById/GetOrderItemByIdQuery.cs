using MediatR;
using Snagged.Application.Catalog.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Queries.GetOrderItemById
{
    public class GetOrderItemByIdQuery:IRequest<OrderItemDto>
    {
        public int Id { get; set; }
    }
}
