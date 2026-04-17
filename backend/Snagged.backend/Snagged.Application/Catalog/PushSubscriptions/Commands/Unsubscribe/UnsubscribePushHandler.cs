using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.PushSubscriptions.Commands.Unsubscribe
{
    public class UnsubscribePushHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<UnsubscribePushCommand>
    {
        public async Task Handle(UnsubscribePushCommand request, CancellationToken ct)
        {
            var userId = currentUser.UserId;

            var subscription = await ctx.PushSubscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Endpoint == request.Endpoint, ct);

            if (subscription is not null)
            {
                ctx.PushSubscriptions.Remove(subscription);
                await ctx.SaveChangesAsync(ct);
            }
        }
    }
}