using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<DeleteNotificationCommand, bool>
    {
        public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notif = await ctx.Notifications.FindAsync(request.Id);

            if (notif == null)
                return false;

            if (notif.UserId != currentUser.UserId)
                throw new UnauthorizedAccessException("You can only delete your own notifications.");

            ctx.Notifications.Remove(notif);
            await ctx.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}