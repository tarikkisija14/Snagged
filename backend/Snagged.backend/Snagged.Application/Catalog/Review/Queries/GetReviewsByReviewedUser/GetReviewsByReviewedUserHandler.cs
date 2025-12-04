
using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewedUser
{
    public class GetReviewsByReviewedUserHandler(IAppDbContext ctx) : IRequestHandler<GetReviewsByReviewedUserQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(GetReviewsByReviewedUserQuery request, CancellationToken cancellationToken)
        {
            return ctx.Reviews
               .Where(r => r.ReviewedUserId == request.ReviewedUserId)
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
