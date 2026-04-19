using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Notifications.Commands.MarkAsRead
{
    public class MarkAsReadHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<MarkAsReadCommand, bool>
    {
        public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var notif = await ctx.Notifications.FindAsync(request.Id);
            if (notif == null) return false;

            if (notif.UserId != currentUser.UserId)
                throw new UnauthorizedAccessException("You can only mark your own notifications as read.");

            notif.IsRead = true;
            await ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}