using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetUsersNotifcations
{
    public class GetUsersNotificationsHandler(IAppDbContext ctx) : IRequestHandler<GetUsersNotificationsQuery, List<NotificationsDto>>
    {
        public async Task<List<NotificationsDto>> Handle(GetUsersNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await ctx.Notifications
               .Where(n => n.UserId == request.UserId)
               .OrderByDescending(n => n.CreatedAt)
               .Select(n => new NotificationsDto
               {
                   Id = n.Id,
                   UserId = n.UserId,
                   Message = n.Message,
                   NotificationType = n.NotificationType,
                   IsRead = n.IsRead,
                   CreatedAt = n.CreatedAt
               })
               .ToListAsync(cancellationToken);
        }
    }
}
