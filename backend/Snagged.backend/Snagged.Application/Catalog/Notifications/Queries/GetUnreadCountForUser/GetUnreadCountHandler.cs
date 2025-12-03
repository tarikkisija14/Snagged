using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetUnreadCountForUser
{
    public class GetUnreadCountHandler(IAppDbContext ctx) : IRequestHandler<GetUnreadCountQuery, int>
    {
        public async Task<int> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
        {
            return await ctx.Notifications
            .Where(n => n.UserId == request.UserId && !n.IsRead)
            .CountAsync(cancellationToken);

        }
    }
}
