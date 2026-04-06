using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewedUser
{
    public class GetReviewsByReviewedUserHandler(IAppDbContext ctx)
        : IRequestHandler<GetReviewsByReviewedUserQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(GetReviewsByReviewedUserQuery request, CancellationToken ct)
        {
            return await ctx.Reviews
                .Where(r => r.ReviewedUserId == request.ReviewedUserId)
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
                .ToListAsync(ct);
        }
    }
}