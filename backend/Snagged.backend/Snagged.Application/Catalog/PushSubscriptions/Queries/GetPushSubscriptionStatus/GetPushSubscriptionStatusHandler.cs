using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.PushSubscriptions.Queries.GetPushSubscriptionStatus
{
    public class GetPushSubscriptionStatusHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<GetPushSubscriptionStatusQuery, bool>
    {
        public async Task<bool> Handle(GetPushSubscriptionStatusQuery request, CancellationToken ct)
        {
            return await ctx.PushSubscriptions
                .AnyAsync(s => s.UserId == currentUser.UserId && s.Endpoint == request.Endpoint, ct);
        }
    }
}