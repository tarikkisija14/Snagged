
using MediatR;

namespace Snagged.Application.Catalog.Items.Queries.GetItemSuggestions
{
    public class GetItemSuggestionsQuery : IRequest<List<ItemSuggestionDto>>
    {
        public string Query { get; set; } = string.Empty;
        public int Limit { get; set; } = 8;
    }
}