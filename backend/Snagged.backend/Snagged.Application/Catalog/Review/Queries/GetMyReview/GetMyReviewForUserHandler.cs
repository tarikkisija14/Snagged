using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Review.Queries.GetMyReview
{
    public class GetMyReviewForUserHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<GetMyReviewForUserQuery, ReviewDto?>
    {
        public async Task<ReviewDto?> Handle(GetMyReviewForUserQuery request, CancellationToken ct)
        {
            return await ctx.Reviews
                .Where(r => r.ReviewerId == currentUser.UserId && r.ReviewedUserId == request.ReviewedUserId)
                .Include(r => r.Reviewer).ThenInclude(u => u!.Profile)
                .Select(r => new ReviewDto
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
                    UpdatedAt = r.UpdatedAt,
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}