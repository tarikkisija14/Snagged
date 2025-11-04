using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Commands.DeleteOrderItem
{
    public class DeleteOrderItemHandler(IAppDbContext ctx) : IRequestHandler<DeleteOrderItemCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteOrderItemCommand request, CancellationToken ct)
        {
            var orderItem = await ctx.OrderItems.FindAsync(new object[] { request.Id }, ct);
            if (orderItem is null)
                throw new KeyNotFoundException($"OrderItem with Id {request.Id} not found.");

            ctx.OrderItems.Remove(orderItem);
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
