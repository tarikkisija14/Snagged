using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Queries.GetAdressesById
{
    public class GetAddressByIdQuery : IRequest<AddressDto?>
    {
        public int Id { get; set; }
        public GetAddressByIdQuery(int id) => Id = id;
    }
}
