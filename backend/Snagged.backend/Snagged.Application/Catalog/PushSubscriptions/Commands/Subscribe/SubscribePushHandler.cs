using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.PushSubscriptions.Commands.Subscribe
{
    public class SubscribePushHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<SubscribePushCommand>
    {
        public async Task Handle(SubscribePushCommand request, CancellationToken ct)
        {
            var userId = currentUser.UserId;

            var existing = await ctx.PushSubscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Endpoint == request.Endpoint, ct);

            if (existing is not null)
            {
                existing.P256DhKey = request.P256DhKey;
                existing.AuthKey = request.AuthKey;
            }
            else
            {
                ctx.PushSubscriptions.Add(new Domain.Entities.PushSubscription
                {
                    UserId = userId,
                    Endpoint = request.Endpoint,
                    P256DhKey = request.P256DhKey,
                    AuthKey = request.AuthKey,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await ctx.SaveChangesAsync(ct);
        }
    }
}