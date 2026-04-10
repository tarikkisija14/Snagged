
namespace Snagged.Application.Catalog.Items.Queries.GetItemSuggestions
{
    public class ItemSuggestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
    }
}