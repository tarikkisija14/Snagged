using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Review.Commands.UpdateReview
{
    public class UpdateReviewHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<UpdateReviewCommand, bool>
    {
        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await ctx.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (review == null) return false;

            if (review.ReviewerId != currentUser.UserId)
                throw new UnauthorizedAccessException("You can only update your own reviews.");

            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            await ctx.SaveChangesAsync(cancellationToken);

            await ReviewRatingService.RecalculateAsync(ctx, review.ReviewedUserId, cancellationToken);

            return true;
        }
    }
}