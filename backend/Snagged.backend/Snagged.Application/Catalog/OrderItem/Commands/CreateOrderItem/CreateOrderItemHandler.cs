using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snagged.Domain.Entities;
namespace Snagged.Application.Catalog.OrderItem.Commands.CreateOrderItem
{
    public class CreateOrderItemHandler(IAppDbContext ctx) :IRequestHandler<CreateOrderItemCommand,int>
    {
        public async Task<int> Handle(CreateOrderItemCommand request, CancellationToken ct)
        {
            var order = await ctx.Orders.FindAsync(new object[] { request.OrderId }, ct);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id {request.OrderId} not found.");

            var orderItem = new Domain.Entities.OrderItem
            {
                OrderId = request.OrderId,
                ItemId = request.Item.ItemId,
                Quantity = request.Item.Quantity,
                Price = request.Item.Price
            };
            
            ctx.OrderItems.Add(orderItem);
            await ctx.SaveChangesAsync(ct);

            return orderItem.Id;


        }
    }
}
