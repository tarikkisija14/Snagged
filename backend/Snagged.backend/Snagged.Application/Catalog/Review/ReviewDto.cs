namespace Snagged.Application.Catalog.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerUsername { get; set; } = string.Empty;
        public string? ReviewerProfileImageUrl { get; set; }
        public int ReviewedUserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}