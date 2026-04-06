using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Notifications.Commands.AddNotification
{
    public class AddNotificationHandler(IAppDbContext ctx) : IRequestHandler<AddNotificationCommand, int>
    {
        public async Task<int> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                UserId = request.UserId,
                Message = request.Message,
                NotificationType = request.NotificationType,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            ctx.Notifications.Add(notification);
            await ctx.SaveChangesAsync(cancellationToken);
            return notification.Id;
        }
    }
}