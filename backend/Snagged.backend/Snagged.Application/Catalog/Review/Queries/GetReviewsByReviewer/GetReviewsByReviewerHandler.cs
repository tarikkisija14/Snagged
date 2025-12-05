using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewer
{
    public class GetReviewsByReviewerHandler : IRequestHandler<GetReviewsByReviewerQuery, List<ReviewDto>>
    {
        private readonly IAppDbContext _ctx;

        public GetReviewsByReviewerHandler(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<ReviewDto>> Handle(GetReviewsByReviewerQuery request, CancellationToken cancellationToken)
        {
            return _ctx.Reviews
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
