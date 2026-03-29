namespace Snagged.Application.Catalog.ItemImages
{
    public class ItemImageDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}