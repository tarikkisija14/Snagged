using MediatR;

namespace Snagged.Application.Catalog.Tags.Queries.GetPopularTags
{
    public class GetPopularTagsQuery : IRequest<List<PopularTagDto>>
    {
        public int Limit { get; init; } = 20;
    }
}