using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.OrderItem.Commands.CreateOrderItem;
using Snagged.Application.Catalog.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Queries.GetOrderItems
{
    public class GetOrderItemsQuery:IRequest<List<OrderItemDto>>
    {
        public int? OrderId { get; set; }  
        public int? ItemId { get; set; }   
    }
}
