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

            var adminRole = await context.Set<Role>().SingleOrDefaultAsync(r => r.RoleName == "Admin");
            var userRole = await context.Set<Role>().SingleOrDefaultAsync(r => r.RoleName == "User");

            if (adminRole == null || userRole == null)
                throw new InvalidOperationException("Roles must exist before seeding users.");

            var users = new List<User>
            {
                new User
                {
                    Email = "admin@snagged.com",
                    PasswordHash = Hasher.HashPassword(null!, "Admin123!"),
                    RoleId = adminRole.Id,
                    Profile = new Profile { Username = "admin", PhoneNumber = "000-000-000", Bio = "Administrator" }
                },
                new User
                {
                    Email = "user1@snagged.com",
                    PasswordHash = Hasher.HashPassword(null!, "User123!"),
                    RoleId = userRole.Id,
                    Profile = new Profile { Username = "user1", PhoneNumber = "111-111-111", Bio = "Thrift shop enthusiast" }
                },
                new User
                {
                    Email = "user2@snagged.com",
                    PasswordHash = Hasher.HashPassword(null!, "User123!"),
                    RoleId = userRole.Id,
                    Profile = new Profile { Username = "user2", PhoneNumber = "222-222-222", Bio = "Loves buying and selling" }
                }
            };

            context.AddRange(users);
            await context.SaveChangesAsync();
        }

        private static async Task SeedAddressesAsync(DatabaseContext context)
        {
            if (await context.Set<Address>().AnyAsync()) return;

            var city = await context.Set<City>().FirstOrDefaultAsync();
            if (city == null) return;

            var users = await context.Set<User>().ToListAsync();

            foreach (var u in users)
            {
                context.Add(new Address
                {
                    UserId = u.Id,
                    CityId = city.Id,
                    Street = "Main St 1",
                    Lat = 43.85m,
                    Lng = 18.38m
                });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedItemsAsync(DatabaseContext context)
        {
            if (await context.Set<Item>().AnyAsync()) return;

            var seller = await context.Set<User>().FirstOrDefaultAsync(u => u.Email == "user1@snagged.com");
            if (seller == null) return;

            var subcategories = await context.Set<Subcategory>().ToListAsync();

            var seeds = new[]
            {
                new { Title="Vintage Denim Jacket", Sub="Coats", Price=45m, Cond="Excellent" },
                new { Title="Summer Floral Dress", Sub="Dresses", Price=30m, Cond="Like New" },
                new { Title="Used Sneakers", Sub="Sneakers", Price=60m, Cond="Good" },
                new { Title="Leather Bag", Sub="Bags", Price=80m, Cond="Very Good" }
            };

            foreach (var s in seeds)
            {
                var sub = subcategories.FirstOrDefault(sc => sc.Name == s.Sub);
                if (sub == null) continue;

                context.Add(new Item
                {
                    UserId = seller.Id,
                    CategoryId = sub.CategoryId,
                    SubcategoryId = sub.Id,
                    Title = s.Title,
                    Description = "Seeded item description",
                    Price = s.Price,
                    Condition = s.Cond,
                    IsSold = false
                });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedCartsOrdersFavoritesReviewsReportsAsync(DatabaseContext context)
        {
            if (await context.Set<Favorite>().AnyAsync()) return;

            var buyer = await context.Set<User>().FirstOrDefaultAsync(u => u.Email == "user2@snagged.com");
            var seller = await context.Set<User>().FirstOrDefaultAsync(u => u.Email == "user1@snagged.com");
            if (buyer == null || seller == null) return;

            var items = await context.Set<Item>().Take(2).ToListAsync();
            foreach (var item in items)
                context.Add(new Favorite { UserId = buyer.Id, ItemId = item.Id });

            context.Add(new Review
            {
                ReviewerId = buyer.Id,
                ReviewedUserId = seller.Id,
                Rating = 5,
                Comment = "Great seller!"
            });

            context.Add(new Report
            {
                ReporterId = buyer.Id,
                ReportedItemId = items.First().Id,
                Reason = "Test Report",
                Status = "Open"
            });

            await context.SaveChangesAsync();
        }

        private static async Task SeedConversationsAndMessagesAsync(DatabaseContext context)
        {
            if (await context.Set<Conversation>().AnyAsync()) return;

            var buyer = await context.Set<User>().FirstOrDefaultAsync(u => u.Email == "user2@snagged.com");
            var seller = await context.Set<User>().FirstOrDefaultAsync(u => u.Email == "user1@snagged.com");
            var item = await context.Set<Item>().FirstOrDefaultAsync();

            if (buyer == null || seller == null || item == null) return;

            var conv = new Conversation { UserId = buyer.Id, ItemId = item.Id, Status = "Open" };
            context.Add(conv);
            await context.SaveChangesAsync();

            context.AddRange(
                new Message { ConversationId = conv.Id, SenderId = buyer.Id, Content = "Is this available?", IsRead = false },
                new Message { ConversationId = conv.Id, SenderId = seller.Id, Content = "Yes it is!", IsRead = false }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedNotificationsAsync(DatabaseContext context)
        {
            if (await context.Set<Notification>().AnyAsync()) return;

            var users = await context.Set<User>().ToListAsync();

            foreach (var user in users)
            {
                context.Add(new Notification
                {
                    UserId = user.Id,
                    Message = "Welcome to Snagged!",
                    NotificationType = "Info",
                    IsRead = false
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
