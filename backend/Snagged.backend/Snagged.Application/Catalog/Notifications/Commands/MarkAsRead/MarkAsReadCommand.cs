using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.MarkAsRead
{
    public class MarkAsReadCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
