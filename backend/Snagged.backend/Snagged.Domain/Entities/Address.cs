using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public string Street { get; set; } = string.Empty;
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        
        
    }
}
