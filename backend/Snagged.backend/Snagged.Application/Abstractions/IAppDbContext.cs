using Microsoft.EntityFrameworkCore;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Abstractions
{
    public interface IAppDbContext
    {
        public DbSet<Country> Countries { get; }
        public DbSet<City> Cities { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<User> Users { get; }
        public DbSet<Profile> Profiles { get; }
        public DbSet<Address> Addresses { get; }
        public DbSet<Category> Categories { get; }
        public DbSet<Subcategory> Subcategories { get; }
        public DbSet<Item> Items { get; }
        public DbSet<ItemImage> ItemImages { get; }
        public DbSet<Order> Orders { get; }
        public DbSet<OrderItem> OrderItems { get; }
        public DbSet<Payment> Payments { get; }
        public DbSet<Favorite> Favorites { get; }
        public DbSet<Review> Reviews { get; }
        public DbSet<Report> Reports { get; }
        public DbSet<Conversation> Conversations { get; }
        public DbSet<Message> Messages { get; }
        public DbSet<Notification> Notifications { get; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
