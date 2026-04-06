using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Paging;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByItem
{
    public class GetReviewsPagedHandler(IAppDbContext ctx)
        : IRequestHandler<GetReviewsPagedQuery, PageResult<ReviewDto>>
    {
        public async Task<PageResult<ReviewDto>> Handle(GetReviewsPagedQuery request, CancellationToken ct)
        {
            var query = ctx.Reviews
                .Where(r => r.ReviewedUserId == request.ReviewedUserId)
                .Include(r => r.Reviewer)
                    .ThenInclude(u => u!.Profile)
                .AsNoTracking();

            query = request.SortOrder switch
            {
                ReviewSortOrder.HighestRating => query.OrderByDescending(r => r.Rating).ThenByDescending(r => r.CreatedAt),
                ReviewSortOrder.LowestRating => query.OrderBy(r => r.Rating).ThenByDescending(r => r.CreatedAt),
                _ => query.OrderByDescending(r => r.CreatedAt)
            };

            var projected = query.Select(r => new ReviewDto
            {
                Id = r.Id,
                ReviewerId = r.ReviewerId,
                ReviewerUsername = r.Reviewer!.Profile != null
                                            ? r.Reviewer.Profile.Username
                                            : r.Reviewer.Email,
                ReviewerProfileImageUrl = r.Reviewer.Profile != null
                                            ? r.Reviewer.Profile.ProfileImageUrl
                                            : null,
                ReviewedUserId = r.ReviewedUserId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });

            return await PageResult<ReviewDto>.FromQueryableAsync(projected, request.Paging, ct);
        }
    }
}