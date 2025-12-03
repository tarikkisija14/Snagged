using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetNotificationById
{
    public class GetNotificationByIdHandler(IAppDbContext ctx) : IRequestHandler<GetNotificationByIdQuery, NotificationsDto>
    {
        public async Task<NotificationsDto> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)

        {
            return await ctx.Notifications
               .Where(n => n.Id == request.Id)
               .Select(n => new NotificationsDto
               {
                   Id = n.Id,
                   UserId = n.UserId,
                   Message = n.Message,
                   NotificationType = n.NotificationType,
                   IsRead = n.IsRead,
                   CreatedAt = n.CreatedAt
               })
               .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
