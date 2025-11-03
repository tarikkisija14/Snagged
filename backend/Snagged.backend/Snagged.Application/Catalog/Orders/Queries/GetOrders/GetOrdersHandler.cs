using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Orders.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IAppDbContext ctx) : IRequestHandler<GetOrdersQuery, List<OrderDto>>
    {
        public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken ct)
        {
            var query = ctx.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .AsQueryable();

            if (request.BuyerId.HasValue)
                query = query.Where(o => o.BuyerId == request.BuyerId.Value);

            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(o => o.Status == request.Status);

            var projectedQuery = query.Select(o => new OrderDto
            {
                Id = o.Id,
                BuyerId = o.BuyerId,
                BuyerEmail = o.Buyer.Email,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.OrderItems.Sum(oi => oi.Price * oi.Quantity),
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    ItemTitle = oi.Item.Title,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            });

            return await projectedQuery.ToListAsync(ct);
        }
    }
}
