using Snagged.Application.Catalog.ItemImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Dto
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Condition { get; set; } = string.Empty;
        public bool IsSold { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ItemImageDto> Images { get; set; } = new();
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }


        public string? CategoryName { get; set; }
        public string? SubcategoryName { get; set; }
        public string? SellerUsername { get; set; }

    }
}
