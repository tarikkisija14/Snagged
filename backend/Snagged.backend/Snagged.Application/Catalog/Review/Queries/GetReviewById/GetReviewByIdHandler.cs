using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewById
{
    public class GetReviewByIdHandler(IAppDbContext ctx)
        : IRequestHandler<GetReviewByIdQuery, ReviewDto?>
    {
        public async Task<ReviewDto?> Handle(GetReviewByIdQuery request, CancellationToken ct)
        {
            return await ctx.Reviews
                .Where(r => r.Id == request.Id)
                .Include(r => r.Reviewer).ThenInclude(u => u!.Profile)
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
                .FirstOrDefaultAsync(ct);
        }
    }
}