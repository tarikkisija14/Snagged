using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Commands.AddReview
{
    public class AddReviewHandler(IAppDbContext ctx) : IRequestHandler<AddReviewCommand, int>
    {
        public async Task<int> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Snagged.Domain.Entities.Review
            {
                ReviewerId = request.ReviewerId,
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
