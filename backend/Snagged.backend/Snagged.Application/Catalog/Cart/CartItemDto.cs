using System;

namespace Snagged.Application.Catalog.Cart
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}