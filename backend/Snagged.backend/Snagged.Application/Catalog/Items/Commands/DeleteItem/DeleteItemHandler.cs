using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Items.Commands.DeleteItem
{
    public class DeleteItemHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<DeleteItemCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken ct)
        {
            var item = await ctx.Items.FindAsync(new object[] { request.Id }, ct);

            if (item is null)
                throw new SnaggedNotFoundException($"Item with id {request.Id} was not found.");

            if (item.UserId != currentUser.UserId)
                throw new UnauthorizedAccessException("You can only delete your own items.");

            ctx.Items.Remove(item);
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}