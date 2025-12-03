using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.MarkAsRead
{
    public class MarkAsReadHandler(IAppDbContext ctx) : IRequestHandler<MarkAsReadCommand, bool>
    {
        public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var notif = await ctx.Notifications.FindAsync(request.Id);
            if (notif == null) return false;

            notif.IsRead = true;
            await ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
