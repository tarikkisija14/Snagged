using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Orders.Commands;
using Snagged.Application.Common.Paging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Queries.GetOrdersFiltered
{
    public class GetFilteredOrdersHandler(IAppDbContext ctx) : IRequestHandler<GetOrdersFilteredQuery, PageResult<OrderDto>>
    {
        public async Task<PageResult<OrderDto>> Handle(GetOrdersFilteredQuery request, CancellationToken ct)
        {
            var query = ctx.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .Include(o => o.Payment)
                .AsQueryable();

            if (request.BuyerId.HasValue)
                query = query.Where(o => o.BuyerId == request.BuyerId.Value);

            
            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(o => o.Status == request.Status);

            
            if (request.OrderDateFrom.HasValue)
                query = query.Where(o => o.OrderDate >= request.OrderDateFrom.Value);
            if (request.OrderDateTo.HasValue)
                query = query.Where(o => o.OrderDate <= request.OrderDateTo.Value);

            
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

            if (request.MinTotalAmount.HasValue)
                projectedQuery = projectedQuery.Where(o => o.TotalAmount >= request.MinTotalAmount.Value);
            if (request.MaxTotalAmount.HasValue)
                projectedQuery = projectedQuery.Where(o => o.TotalAmount <= request.MaxTotalAmount.Value);

            if (request.PaymentId.HasValue)
                projectedQuery = projectedQuery.Where(o => o.Id == request.PaymentId.Value); 
            else if (request.IsPaid.HasValue)
                projectedQuery = request.IsPaid.Value
                    ? projectedQuery.Where(o => o.Id != null)  
                    : projectedQuery.Where(o => o.Id == null);
            
            return await PageResult<OrderDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
