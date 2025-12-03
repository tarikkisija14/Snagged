using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetUsersNotifcations
{
    public class GetUsersNotificationsQuery : IRequest<List<NotificationsDto>>
    {
        public int UserId { get; set; }
    }
}
