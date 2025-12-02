using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Queries.GetAdresses
{
    public class GetAddressesQuery:IRequest<List<AddressDto>>
    {
        public int? UserId { get; set; }
    }
}
