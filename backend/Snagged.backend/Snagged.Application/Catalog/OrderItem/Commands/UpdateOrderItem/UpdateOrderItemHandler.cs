using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Commands.UpdateOrderItem
{
    public class UpdateOrderItemHandler(IAppDbContext ctx) : IRequestHandler<UpdateOrderItemCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderItemCommand request, CancellationToken ct)
        {
            var orderItem = await ctx.OrderItems.FindAsync(new object[] { request.Id }, ct);
            if (orderItem is null)
                throw new KeyNotFoundException($"OrderItem with Id {request.Id} not found.");

            orderItem.Quantity = request.Item.Quantity;
            orderItem.Price = request.Item.Price;

            await ctx.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
