using Microsoft.EntityFrameworkCore;
using Snagged.Application.Catalog.ItemImages;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Items
{
    public static class ItemProjectionExtensions
    {
        public static IQueryable<ItemDto> ProjectToDto(this IQueryable<Item> query)
        {
            return query.Select(i => new ItemDto
            {
                Id = i.Id,
                UserId = i.UserId,
                Title = i.Title,
                Description = i.Description,
                Price = i.Price,
                Condition = i.Condition,
                IsSold = i.IsSold,
                CreatedAt = i.CreatedAt,
                CategoryId = i.CategoryId,
                SubcategoryId = i.SubcategoryId,
                LikesCount = i.Favorites.Count,
                CategoryName = i.Category.Name,
                SubcategoryName = i.Subcategory != null ? i.Subcategory.Name : null,
                SellerUsername = i.User.Profile != null ? i.User.Profile.Username : i.User.Email,
                Images = i.Images
                    .OrderByDescending(img => img.IsMain)
                    .ThenBy(img => img.Id)
                    .Select(img => new ItemImageDto
                    {
                        Id = img.Id,
                        ItemId = img.ItemId,
                        ImageUrl = img.ImageUrl,
                        IsMain = img.IsMain
                    })
                    .ToList()
            });
        }

        public static IQueryable<Item> IncludeItemDetails(this IQueryable<Item> query)
        {
            return query
                .Include(i => i.Category)
                .Include(i => i.Subcategory)
                .Include(i => i.User).ThenInclude(u => u.Profile)
                .Include(i => i.Images)
                .Include(i => i.Favorites);
        }
    }
}