using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;

public static class TestDataSeeder
{
    public static async Task SeedAsync(IAppDbContext ctx)
    {
        if (!ctx.Users.Any())
            ctx.Users.Add(new User { Id = 1, Email = "test@test.com" });

        if (!ctx.Categories.Any())
            ctx.Categories.Add(new Category { Id = 1, Name = "TestCat" });

        if (!ctx.Countries.Any())
            ctx.Countries.Add(new Country { Id = 1, Name = "TestCountry" });

        if (!ctx.Cities.Any())
            ctx.Cities.Add(new City { Id = 1, Name = "TestCity", CountryId = 1 });

        if (!ctx.Items.Any())
            ctx.Items.Add(new Item { Id = 1, Title = "TestItem", Price = 10, CategoryId = 1, UserId = 1, Condition = "New" });

        if (!ctx.ItemImages.Any())
            ctx.ItemImages.Add(new ItemImage { Id = 1, ItemId = 1, ImageUrl = "url.jpg" });

        if (!ctx.Carts.Any())
            ctx.Carts.Add(new Cart { Id = 1, UserId = 1 });

        if (!ctx.CartItems.Any())
            ctx.CartItems.Add(new CartItem { Id = 1, CartId = 1, ItemId = 1, Quantity = 1 });

        if (!ctx.Addresses.Any())
            ctx.Addresses.Add(new Address { Id = 1, Street = "Street 1", UserId = 1 });

        await ctx.SaveChangesAsync();
    }
}
