using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Queries.GetOrderItemById
{
    public class GetOrderItemByIdHandler(IAppDbContext ctx) : IRequestHandler<GetOrderItemByIdQuery, OrderItemDto>
    {
        public async Task<OrderItemDto> Handle(GetOrderItemByIdQuery request, CancellationToken ct)
        {
            var item = await ctx.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (item == null)
                throw new KeyNotFoundException($"Order item with Id {request.Id} not found.");

            return new OrderItemDto
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                Price = item.Price
            };
        }
    }
}
