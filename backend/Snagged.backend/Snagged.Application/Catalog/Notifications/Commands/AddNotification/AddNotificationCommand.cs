using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.AddNotification
{
    public class AddNotificationCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public string NotificationType { get; set; }
    }
}
