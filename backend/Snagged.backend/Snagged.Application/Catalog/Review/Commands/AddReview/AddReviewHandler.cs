using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Review.Commands.AddReview
{
    public class AddReviewHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<AddReviewCommand, int>
    {
        public async Task<int> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Snagged.Domain.Entities.Review
            {
                ReviewerId = currentUser.UserId,
                ReviewedUserId = request.ReviewedUserId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            ctx.Reviews.Add(review);
            await ctx.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
}