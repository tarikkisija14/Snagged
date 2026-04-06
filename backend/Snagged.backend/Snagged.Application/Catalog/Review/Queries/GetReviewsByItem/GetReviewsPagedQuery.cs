using MediatR;

using Snagged.Application.Common.Paging;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByItem
{
    public class GetReviewsPagedQuery : IRequest<PageResult<ReviewDto>>
    {
        public int ReviewedUserId { get; set; }
        public PageRequest Paging { get; init; } = new();
        public ReviewSortOrder SortOrder { get; init; } = ReviewSortOrder.Newest;
    }
}