using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses
{
    public class AddressDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string Street { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
    }
}
