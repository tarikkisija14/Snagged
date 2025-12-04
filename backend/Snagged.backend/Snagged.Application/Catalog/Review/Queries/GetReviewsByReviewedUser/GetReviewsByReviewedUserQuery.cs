using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewedUser
{
    public class GetReviewsByReviewedUserQuery : IRequest<List<ReviewDto>>
    {
        public int ReviewedUserId { get; set; }
    }
}
