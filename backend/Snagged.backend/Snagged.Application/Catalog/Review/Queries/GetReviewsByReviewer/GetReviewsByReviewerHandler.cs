using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewerQuery
{
    public class GetReviewsByReviewerHandler(IAppDbContext ctx) : IRequestHandler<GetReviewsByReviewerQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(GetReviewsByReviewerQuery request, CancellationToken cancellationToken)
        {
            return ctx.Reviews
                .Where(r => r.ReviewerId == request.ReviewerId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerId = r.ReviewerId,
                    ReviewedUserId = r.ReviewedUserId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }).ToList();
        }
    }
}
