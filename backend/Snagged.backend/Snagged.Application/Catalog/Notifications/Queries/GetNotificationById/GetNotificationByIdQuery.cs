using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetNotificationById
{
    public class GetNotificationByIdQuery : IRequest<NotificationsDto>
    {
        public int Id { get; set; }
    }
}
