namespace Snagged.Application.Catalog.Cart
{
    internal static class CartProjections
    {
        internal static CartDto ToDto(this Domain.Entities.Cart c, bool isSavedForLater) =>
            new CartDto
            {
                Id = c.Id,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                IsSavedForLater = isSavedForLater,
                Items = c.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ItemId = ci.ItemId,
                    ItemName = ci.Item.Title,
                    ImageUrl = ci.Item.Images
                                  .Where(img => img.IsMain)
                                  .Select(img => img.ImageUrl)
                                  .FirstOrDefault()
                               ?? ci.Item.Images
                                  .Select(img => img.ImageUrl)
                                  .FirstOrDefault()
                               ?? string.Empty,
                    Price = ci.Item.Price,
                    Quantity = ci.Quantity,
                    AddedAt = ci.AddedAt
                }).ToList()
            };
    }
}