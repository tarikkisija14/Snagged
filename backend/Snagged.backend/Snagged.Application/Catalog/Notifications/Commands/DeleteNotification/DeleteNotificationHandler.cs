using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationHandler(IAppDbContext ctx) : IRequestHandler<DeleteNotificationCommand, bool>
    {
        public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notif = await ctx.Notifications.FindAsync(request.Id);

            if (notif == null)
                return false;

            ctx.Notifications.Remove(notif);
            await ctx.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
