using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snagged.Domain.Entities;

namespace Snagged.Infrastructure.Database.Seeders
{
    public static class DynamicDataSeeder
    {
        private static readonly PasswordHasher<User> Hasher = new();

        public static async Task SeedAsync(DatabaseContext context)
        {
            await SeedUsersAndProfilesAsync(context);
            await SeedAddressesAsync(context);
            await SeedItemsAsync(context);
            await SeedCartsOrdersFavoritesReviewsReportsAsync(context);
            await SeedConversationsAndMessagesAsync(context);
            await SeedNotificationsAsync(context);
        }

        private static async Task SeedUsersAndProfilesAsync(DatabaseContext context)
        {
            if (await context.Set<User>().AnyAsync()) return;

            var adminRole = await context.Set<Role>().SingleAsync(r => r.RoleName == "Admin");
            var userRole = await context.Set<Role>().SingleAsync(r => r.RoleName == "User");

            var users = new List<User>
            {
                new User
                {
                    Email = "admin@snagged.com",
                    PasswordHash = Hasher.HashPassword(null!, "Admin123!"),
                    RoleId = adminRole.Id,
                    Profile = new Profile
                    {
                        Username = "admin",
                        PhoneNumber = "000-000-000",
                        Bio = "Administrator account"
                    }
                },
                new User
                {
                    Email = "prodavac@snagged.com",
                    PasswordHash = Hasher.HashPassword(null!, "Prodavac123!"),
                    RoleId = userRole.Id,
                    Profile = new Profile
                    {
                        Username = "prodavac",
                        PhoneNumber = "111-111-111",
                        Bio = "Seller account"
                    }
                },
                new User
                {
                    Email = "kupac@snagged.com",
                    PasswordHash = Hasher.HashPassword(null!, "Kupac123!"),
                    RoleId = userRole.Id,
                    Profile = new Profile
                    {
                        Username = "kupac",
                        PhoneNumber = "222-222-222",
                        Bio = "Buyer account"
                    }
                }
            };

            context.AddRange(users);
            await context.SaveChangesAsync();
        }

        private static async Task SeedAddressesAsync(DatabaseContext context)
        {
            if (await context.Set<Address>().AnyAsync()) return;

            var city = await context.Set<City>().FirstAsync();
            var users = await context.Set<User>().ToListAsync();

            foreach (var u in users)
            {
                context.Add(new Address
                {
                    UserId = u.Id,
                    CityId = city.Id,
                    Street = "Some street 123",
                    Lat = 43.85m,
                    Lng = 18.38m
                });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedItemsAsync(DatabaseContext context)
        {
            if (await context.Set<Item>().AnyAsync()) return;

            var seller = await context.Set<User>().SingleAsync(u => u.Email == "prodavac@snagged.com");
            var subcategories = await context.Set<Subcategory>().ToListAsync();

            var seeds = new[]
            {
                new { Title="Vintage Denim Jacket", Sub="Men's Clothing", Desc="Classic 90s style.", Price=45m, Cond="Excellent" },
                new { Title="Summer Floral Dress", Sub="Women's Clothing", Desc="Light summer dress.", Price=30m, Cond="Like New" },
                new { Title="Used Sneakers", Sub="Sneakers", Desc="Cleaned, good condition.", Price=60m, Cond="Good" },
                new { Title="Leather Shoulder Bag", Sub="Bags", Desc="Genuine leather bag.", Price=80m, Cond="Very Good" },
                new { Title="Winter Boots", Sub="Boots", Desc="Warm and durable.", Price=70m, Cond="Good" }
            };

            foreach (var s in seeds)
            {
                var sub = subcategories.Single(sc => sc.Name == s.Sub);

                context.Add(new Item
                {
                    UserId = seller.Id,
                    CategoryId = sub.CategoryId,
                    SubcategoryId = sub.Id,
                    Title = s.Title,
                    Description = s.Desc,
                    Price = s.Price,
                    Condition = s.Cond,
                    IsSold = false
                });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedCartsOrdersFavoritesReviewsReportsAsync(DatabaseContext context)
        {
            var buyer = await context.Set<User>().SingleAsync(u => u.Email == "kupac@snagged.com");
            var seller = await context.Set<User>().SingleAsync(u => u.Email == "prodavac@snagged.com");
            var items = await context.Set<Item>().Take(3).ToListAsync();

            // Favorites
            foreach (var item in items)
            {
                if (!await context.Set<Favorite>().AnyAsync(f => f.UserId == buyer.Id && f.ItemId == item.Id))
                    context.Add(new Favorite { UserId = buyer.Id, ItemId = item.Id });
            }

            // Reviews
            if (!await context.Set<Review>().AnyAsync(r => r.ReviewerId == buyer.Id && r.ReviewedUserId == seller.Id))
            {
                context.Add(new Review
                {
                    ReviewerId = buyer.Id,
                    ReviewedUserId = seller.Id,
                    Rating = 5,
                    Comment = "Fast and efficient cooperation!"
                });
            }

            // Reports (dummy example)
            context.Add(new Report
            {
                ReporterId = buyer.Id,
                ReportedItemId = items.First().Id,
                Reason = "Test report",
                Status = "Open"
            });

            await context.SaveChangesAsync();
        }

        private static async Task SeedConversationsAndMessagesAsync(DatabaseContext context)
        {
            var buyer = await context.Set<User>().SingleAsync(u => u.Email == "kupac@snagged.com");
            var seller = await context.Set<User>().SingleAsync(u => u.Email == "prodavac@snagged.com");
            var item = await context.Set<Item>().FirstAsync();

            var conv = new Conversation
            {
                UserId = buyer.Id,
                ItemId = item.Id,
                Status = "Open"
            };
            context.Add(conv);
            await context.SaveChangesAsync();

            context.Add(new Message
            {
                ConversationId = conv.Id,
                SenderId = buyer.Id,
                Content = "Hello, interested in this item.",
                IsRead = false
            });

            context.Add(new Message
            {
                ConversationId = conv.Id,
                SenderId = seller.Id,
                Content = "Hi! Yes, it's available.",
                IsRead = false
            });

            await context.SaveChangesAsync();
        }

        private static async Task SeedNotificationsAsync(DatabaseContext context)
        {
            var buyer = await context.Set<User>().SingleAsync(u => u.Email == "kupac@snagged.com");

            context.Add(new Notification
            {
                UserId = buyer.Id,
                Message = "Welcome to Snagged!",
                NotificationType = "Info",
                IsRead = false
            });

            await context.SaveChangesAsync();
        }
    }
}
