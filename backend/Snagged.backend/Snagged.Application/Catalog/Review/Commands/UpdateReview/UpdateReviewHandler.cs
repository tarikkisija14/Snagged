using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Commands.UpdateReview
{
    public class UpdateReviewHandler(IAppDbContext ctx) : IRequestHandler<UpdateReviewCommand, bool>
    {
        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await ctx.Reviews.FindAsync(request.Id);
            if (review == null) return false;

            review.Rating = request.Rating;
            review.Comment = request.Comment;

            await ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
