using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;



namespace Snagged.Infrastructure.Database
{
    public class DatabaseContext : DbContext, IAppDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

            // USER
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(200);
                entity.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // PROFILE
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("Profiles");
                entity.HasIndex(p => p.Username).IsUnique();
                entity.HasIndex(p => p.UserId).IsUnique();
                entity.Property(p => p.Username).IsRequired().HasMaxLength(50);
                entity.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(p => p.ProfileImageUrl).HasMaxLength(500);
                entity.Property(p => p.Bio).HasMaxLength(1000);
                entity.Property(p => p.AverageRating).HasPrecision(3, 2);
            });

            // ITEM
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Items");
                entity.HasIndex(i => i.Title);
                entity.HasIndex(i => i.CategoryId);
                entity.HasIndex(i => i.UserId);
                entity.HasIndex(i => i.CreatedAt);
                entity.Property(i => i.Title).IsRequired().HasMaxLength(200);
                entity.Property(i => i.Description).IsRequired().HasMaxLength(2000);
                entity.Property(i => i.Price).IsRequired().HasPrecision(18, 2);
                entity.Property(i => i.Condition).IsRequired().HasMaxLength(50);
                entity.Property(i => i.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.ToTable("Favorites");
                entity.HasIndex(f => new { f.UserId, f.ItemId }).IsUnique();
                entity.Property(f => f.AddedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // ADDRESS
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Addresses");
                entity.HasIndex(a => a.UserId);
                entity.Property(a => a.Street).IsRequired().HasMaxLength(200);
                entity.Property(a => a.Lat).HasPrecision(9, 6);
                entity.Property(a => a.Lng).HasPrecision(9, 6);
            });

            // CATEGORY
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            // SUBCATEGORY
            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.ToTable("Subcategories");
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            });

            // CITY
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("Cities");
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            // COUNTRY
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries");
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            // CONVERSATION
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.ToTable("Conversations");
                entity.Property(c => c.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Active");
                entity.Property(c => c.StartedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // ITEM IMAGE
            modelBuilder.Entity<ItemImage>(entity =>
            {
                entity.ToTable("ItemImages");
                entity.Property(i => i.ImageUrl).IsRequired().HasMaxLength(500);
            });

            // MESSAGE
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Messages");
                entity.HasIndex(m => m.ConversationId);
                entity.HasIndex(m => m.SentAt);
                entity.Property(m => m.Content).IsRequired();
                entity.Property(m => m.SentAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // NOTIFICATION
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications");
                entity.HasIndex(n => n.UserId);
                entity.Property(n => n.Message).IsRequired().HasMaxLength(500);
                entity.Property(n => n.NotificationType).IsRequired().HasMaxLength(50);
                entity.Property(n => n.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // ORDER
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasIndex(o => o.BuyerId);
                entity.HasIndex(o => o.OrderDate);
                entity.HasIndex(o => o.Status);
                entity.Property(o => o.Status).IsRequired().HasMaxLength(50);
                entity.Property(o => o.OrderDate).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(o => o.StripePaymentIntentId).HasMaxLength(100);
            });

            // ORDER ITEM
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.Property(oi => oi.Price).IsRequired().HasPrecision(18, 2);
                entity.Property(oi => oi.Quantity).IsRequired();
            });

            // PAYMENT
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");
                entity.HasIndex(p => p.StripePaymentIntentId).IsUnique();
                entity.HasIndex(p => p.StripeChargeId).IsUnique();
                entity.Property(p => p.PaymentMethod).HasMaxLength(50);
                entity.Property(p => p.PaidAmount).IsRequired().HasPrecision(18, 2);
                entity.Property(p => p.StripePaymentIntentId).IsRequired().HasMaxLength(100);
                entity.Property(p => p.StripeChargeId).HasMaxLength(100);
                entity.Property(p => p.PaymentDate).IsRequired();
            });

            // REPORT
            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Reports");
                entity.Property(r => r.Reason).IsRequired().HasMaxLength(1000);
                entity.Property(r => r.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
                entity.Property(r => r.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // REVIEW
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews");
                entity.Property(r => r.Comment).IsRequired().HasMaxLength(2000);
                entity.Property(r => r.Rating).IsRequired();
                entity.Property(r => r.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // ROLE
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.Property(r => r.RoleName).IsRequired().HasMaxLength(50);
                entity.Property(r => r.RoleDescription).HasMaxLength(200);
            });

            // CART
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Carts");
                entity.HasIndex(c => c.UserId).IsUnique();
                entity.Property(c => c.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(c => c.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // CART ITEM
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItems");
                entity.Property(ci => ci.Quantity).IsRequired();
                entity.Property(ci => ci.AddedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });


            // User → Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull);

            // User → Profile (1:1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);

            // User → Cart (1:1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);

            // City → Country
            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany(c => c.Cities)
                .HasForeignKey(c => c.CountryId);

            // Address → City
            modelBuilder.Entity<Address>()
                .HasOne(a => a.City)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CityId);

            // Address → User
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);

            // Item → Category
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId);

            // Item → Subcategory
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Subcategory)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SubcategoryId)
                .IsRequired(false);

            // Item → User
            modelBuilder.Entity<Item>()
                .HasOne(i => i.User)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.UserId);

            // ItemImage → Item
            modelBuilder.Entity<ItemImage>()
                .HasOne(ii => ii.Item)
                .WithMany(i => i.Images)
                .HasForeignKey(ii => ii.ItemId);

            // CartItem → Cart
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem → Item
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Item)
                .WithMany(i => i.CartItems)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order → Buyer (User)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.BuyerId);

            // Payment ↔ Order (1:1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .IsRequired();

            // OrderItem → Order
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem → Item
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Favorite → User
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Favorite → Item
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Item)
                .WithMany(i => i.Favorites)
                .HasForeignKey(f => f.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review → Reviewer (User)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.ReviewsGiven)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review → ReviewedUser
            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewedUser)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(r => r.ReviewedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Report → Reporter
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.ReporterId);

            // Report → ReportedItem
            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedItem)
                .WithMany(i => i.Reports)
                .HasForeignKey(r => r.ReportedItemId)
                .IsRequired(false);

            // Report → ReportedUser
            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedUser)
                .WithMany()
                .HasForeignKey(r => r.ReportedUserId)
                .IsRequired(false);

            // Conversation → User
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User)
                .WithMany(u => u.Conversations)
                .HasForeignKey(c => c.UserId);

            // Conversation → Item
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Item)
                .WithMany(i => i.Conversations)
                .HasForeignKey(c => c.ItemId)
                .IsRequired(false);

            // Message → Conversation
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message → Sender
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification → User
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

          
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
            configurationBuilder.Properties<decimal?>().HavePrecision(18, 2);
            configurationBuilder.Properties<string>().HaveMaxLength(255);
        }
    }
}