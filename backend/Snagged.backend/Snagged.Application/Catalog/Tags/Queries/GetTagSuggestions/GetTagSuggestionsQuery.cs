using MediatR;

namespace Snagged.Application.Catalog.Tags.Queries.GetTagSuggestions
{
    public class GetTagSuggestionsQuery : IRequest<List<TagSuggestionDto>>
    {
        public string Query { get; init; } = string.Empty;
        public int Limit { get; init; } = 10;
    }
}