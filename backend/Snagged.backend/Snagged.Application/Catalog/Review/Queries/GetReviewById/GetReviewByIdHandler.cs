using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewById
{
    public class GetReviewByIdHandler(IAppDbContext ctx) : IRequestHandler<GetReviewByIdQuery, ReviewDto>
    {
        public async Task<ReviewDto>Handle (GetReviewByIdQuery request,CancellationToken ct)
        {
            var review = await ctx.Reviews.FindAsync(request.Id);
            if (review == null) return null;

            return new ReviewDto
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewedUserId = review.ReviewedUserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
