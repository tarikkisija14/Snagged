using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler(IAppDbContext ctx) : IRequestHandler<DeleteOrderCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken ct)
        {
            var order = await ctx.Orders.FindAsync(new object[] { request.Id }, ct);
            if (order == null)
                throw new KeyNotFoundException($"Order with Id {request.Id} not found.");

            ctx.Orders.Remove(order);
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
