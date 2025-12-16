using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
        public string ImageUrl { get; set; } 
        public decimal Price { get; set; }    

    }
}
