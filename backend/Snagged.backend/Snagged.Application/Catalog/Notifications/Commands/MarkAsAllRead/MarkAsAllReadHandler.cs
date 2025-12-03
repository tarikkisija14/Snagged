using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.MarkAsAllRead
{
    public class MarkAsAllReadHandler(IAppDbContext ctx) : IRequestHandler<MarkAsAllReadCommand, int>
    {
        public async Task<int> Handle(MarkAsAllReadCommand request, CancellationToken cancellationToken)
        {
            var notifications = await ctx.Notifications
                .Where(n => n.UserId == request.UserId && !n.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var n in notifications) n.IsRead = true;

            await ctx.SaveChangesAsync(cancellationToken);
            return notifications.Count;
        }
    }
}
