using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Items.Commands.DeleteItem
{
    public class DeleteItemHandler(IAppDbContext ctx):IRequestHandler<DeleteItemCommand,Unit>    
    {
        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken ct)
        {
            var item = await ctx.Items.FindAsync(new object[] { request.Id }, ct);
            if (item == null)
                throw new KeyNotFoundException($"Item with id {request.Id} not found.");

            ctx.Items.Remove(item);

            await ctx.SaveChangesAsync(ct);
            return Unit.Value;
        }

    }
}
