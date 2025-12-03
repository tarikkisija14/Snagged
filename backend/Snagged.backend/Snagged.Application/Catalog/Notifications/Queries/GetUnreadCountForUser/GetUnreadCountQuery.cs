using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetUnreadCountForUser
{
    public class GetUnreadCountQuery : IRequest<int>
    {
        public int UserId
        {
            get; set;
        }
    }
}
