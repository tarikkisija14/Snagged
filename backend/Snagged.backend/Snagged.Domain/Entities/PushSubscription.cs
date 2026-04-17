namespace Snagged.Domain.Entities
{
    public class PushSubscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Endpoint { get; set; } = string.Empty;
        public string P256DhKey { get; set; } = string.Empty;
        public string AuthKey { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}