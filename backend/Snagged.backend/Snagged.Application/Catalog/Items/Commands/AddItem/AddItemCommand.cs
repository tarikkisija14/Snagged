using MediatR;

namespace Snagged.Application.Catalog.Items.Commands.AddItem
{
    public class AddItemCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }

        public List<string> ImageUrls { get; set; } = new();
        public List<string> Tags { get; set; } = new();
    }
}