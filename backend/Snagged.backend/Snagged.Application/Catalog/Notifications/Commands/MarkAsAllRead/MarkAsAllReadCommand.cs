using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.MarkAsAllRead
{
    public class MarkAsAllReadCommand : IRequest<int>
    {
        public int UserId { get; set; }
    }
}
