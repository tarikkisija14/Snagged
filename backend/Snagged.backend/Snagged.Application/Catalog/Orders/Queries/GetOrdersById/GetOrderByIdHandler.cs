using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Orders.Commands;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Orders.Queries.GetOrdersById
{
    public class GetOrderByIdHandler(IAppDbContext ctx) : IRequestHandler<GetOrdersByIdQuery, OrderDto>
    {
        public async Task<OrderDto> Handle(GetOrdersByIdQuery request, CancellationToken ct)
        {
            var order = await ctx.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.Id == request.Id, ct);

            if (order == null)
                throw new SnaggedNotFoundException($"Order with id {request.Id} not found.");

            return new OrderDto
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                BuyerEmail = order.Buyer.Email,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity),
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    ItemTitle = oi.Item.Title,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }
    }
}