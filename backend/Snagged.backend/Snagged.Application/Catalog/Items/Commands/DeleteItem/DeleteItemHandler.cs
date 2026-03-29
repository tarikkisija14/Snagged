using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Items.Commands.DeleteItem
{
    public class DeleteItemHandler(IAppDbContext ctx) : IRequestHandler<DeleteItemCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken ct)
        {
            var item = await ctx.Items.FindAsync(new object[] { request.Id }, ct);

            if (item is null)
                throw new SnaggedNotFoundException($"Item with id {request.Id} was not found.");

            ctx.Items.Remove(item);
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}