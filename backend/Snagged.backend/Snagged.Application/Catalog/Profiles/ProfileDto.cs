namespace Snagged.Application.Catalog.Profiles
{
    public class ProfileDto
    {
        public string Username { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string Bio { get; set; } = string.Empty;
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
