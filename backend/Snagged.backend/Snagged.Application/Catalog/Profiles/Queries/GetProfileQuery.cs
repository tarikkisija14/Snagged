using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Profiles.Queries
{
    public class GetProfileQuery : IRequest<ProfileDto>
    {
        public int UserId { get; set; }
    }
}
