using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IAppDbContext ctx) : IRequestHandler<UpdateOrderCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken ct)
        {
            var order = await ctx.Orders.FindAsync(new object[] { request.Id }, ct);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id {request.Id} not found.");

            order.Status = request.Status;

            await ctx.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
