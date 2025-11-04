using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Queries.GetOrderItemsByOrderId
{
    public class GetOrderItemsByOrderIdHandler(IAppDbContext ctx)
        : IRequestHandler<GetOrderItemsByOrderIdQuery, List<OrderItemDto>>
    {
        public async Task<List<OrderItemDto>> Handle(GetOrderItemsByOrderIdQuery request, CancellationToken ct)
        {
            var items = await ctx.OrderItems
                .Where(x => x.OrderId == request.OrderId)
                .Select(x => new OrderItemDto
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ItemId = x.ItemId,
                    Quantity = x.Quantity,
                    Price = x.Price
                })
                .ToListAsync(ct);

            return items;
        }
    }
}
