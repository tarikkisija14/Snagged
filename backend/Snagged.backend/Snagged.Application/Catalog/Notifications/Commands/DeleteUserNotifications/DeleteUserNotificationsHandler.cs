using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.DeleteUserNotifications
{
    public class DeleteUserNotificationsHandler(IAppDbContext ctx) : IRequestHandler<DeleteUserNotificationsCommand, int>
    {
        public async Task<int> Handle(DeleteUserNotificationsCommand request, CancellationToken cancellationToken)
        {
            var list = await ctx.Notifications
             .Where(n => n.UserId == request.UserId)
             .ToListAsync(cancellationToken);

            ctx.Notifications.RemoveRange(list);
            await ctx.SaveChangesAsync(cancellationToken);

            return list.Count;

        }
    }
}
