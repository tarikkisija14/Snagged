using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                CreatedAt = DateTime.Now
            };

            ctx.Notifications.Add(notification);
            await ctx.SaveChangesAsync(cancellationToken);
            return notification.Id;
        }
    }
}
