namespace Snagged.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Password {  get; set; }   

        public DateTime CreatedAt { get; set; }=DateTime.Now;

        public int? RoleId { get; set; }

        public Role Role { get; set; }

        public Profile Profile { get; set; }

        public Cart Cart { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Review> ReviewsGiven { get; set; } = new List<Review>();
        public ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();


    }
}
