using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
