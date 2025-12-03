using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.DeleteUserNotifications
{
    public class DeleteUserNotificationsCommand : IRequest<int>
    {
        public int UserId { get; set; }
    }
}
