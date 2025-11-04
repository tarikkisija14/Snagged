using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Queries.GetItems
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Condition { get; set; }
        public bool IsSold { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public string SellerUsername { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }
}
