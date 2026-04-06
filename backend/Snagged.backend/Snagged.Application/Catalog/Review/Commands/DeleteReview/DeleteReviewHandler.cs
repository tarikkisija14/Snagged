using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Review.Commands.DeleteReview
{
    public class DeleteReviewHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<DeleteReviewCommand, bool>
    {
        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await ctx.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (review == null) return false;

            if (review.ReviewerId != currentUser.UserId)
                throw new UnauthorizedAccessException("You can only delete your own reviews.");

            var reviewedUserId = review.ReviewedUserId;
            ctx.Reviews.Remove(review);
            await ctx.SaveChangesAsync(cancellationToken);

            await ReviewRatingService.RecalculateAsync(ctx, reviewedUserId, cancellationToken);

            return true;
        }
    }
}