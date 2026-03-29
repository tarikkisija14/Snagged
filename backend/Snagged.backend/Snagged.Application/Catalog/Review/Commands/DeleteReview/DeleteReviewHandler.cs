using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Review.Commands.DeleteReview
{
    public class DeleteReviewHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<DeleteReviewCommand, bool>
    {
        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await ctx.Reviews.FindAsync(request.Id);
            if (review == null) return false;

            if (review.ReviewerId != currentUser.UserId)
                throw new UnauthorizedAccessException("You can only delete your own reviews.");

            ctx.Reviews.Remove(review);
            await ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}