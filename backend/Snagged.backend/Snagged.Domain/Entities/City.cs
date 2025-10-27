using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public int CountryId {  get; set; }
        public Country Country { get; set; }
        public ICollection<Adress> Adressses { get; set; } = new List<Adresss>();
    }
}
