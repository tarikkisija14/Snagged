using MediatR;

namespace Snagged.Application.Catalog.Items.Queries.GetItemSuggestions
{
    public class GetItemSuggestionsQuery : IRequest<List<string>>
    {
        public string Query { get; set; } = string.Empty;
    }
}