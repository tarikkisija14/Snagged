using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewer
{
    public class GetReviewsByReviewerHandler(IAppDbContext ctx)
        : IRequestHandler<GetReviewsByReviewerQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(GetReviewsByReviewerQuery request, CancellationToken cancellationToken)
        {
            return await ctx.Reviews
                .Where(r => r.ReviewerId == request.ReviewerId)
                .Include(r => r.Reviewer).ThenInclude(u => u!.Profile)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerId = r.ReviewerId,
                    ReviewerUsername = r.Reviewer!.Profile!.Username,
                    ReviewerProfileImageUrl = r.Reviewer.Profile.ProfileImageUrl,
                    ReviewedUserId = r.ReviewedUserId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}