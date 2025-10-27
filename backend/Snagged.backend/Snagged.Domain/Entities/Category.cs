using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
