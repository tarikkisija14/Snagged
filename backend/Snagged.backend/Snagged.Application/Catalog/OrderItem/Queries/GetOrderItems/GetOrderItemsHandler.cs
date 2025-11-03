using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Queries.GetOrderItems
{
    public class GetOrderItemsHandler(IAppDbContext ctx) : IRequestHandler<GetOrderItemsQuery, List<OrderItemDto>>
    {
        public async Task<List<OrderItemDto>> Handle(GetOrderItemsQuery request, CancellationToken ct)
        {
            var query = ctx.OrderItems.AsQueryable();

            if (request.OrderId.HasValue)
                query = query.Where(x => x.OrderId == request.OrderId.Value);

            if (request.ItemId.HasValue)
                query = query.Where(x => x.ItemId == request.ItemId.Value);

            return await query
                .Select(x => new OrderItemDto
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ItemId = x.ItemId,
                    Quantity = x.Quantity,
                    Price = x.Price
                })
                .ToListAsync(ct);
        }
    }
}
