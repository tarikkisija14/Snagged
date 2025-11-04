using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler(IAppDbContext ctx) : IRequestHandler<CreateOrderCommand, int>
    {
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            var order = new Order
            {
                BuyerId = request.Order.BuyerId,
                Status = request.Order.Status,
                OrderDate = DateTime.Now,
                OrderItems = request.Order.Items.Select(i => new Snagged.Domain.Entities.OrderItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
            ctx.Orders.Add(order);
            await ctx.SaveChangesAsync(ct);

            return order.Id;
        }
    }
}
