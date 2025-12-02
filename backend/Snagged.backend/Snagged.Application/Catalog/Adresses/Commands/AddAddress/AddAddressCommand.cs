using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Commands.AddAddress
{
    public class AddAddressCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int CityId { get; set; }
        public string Street { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
    }
}
