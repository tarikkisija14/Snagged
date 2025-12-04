using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewerQuery
{
    public class GetReviewsByReviewerQuery : IRequest<List<ReviewDto>>
    {
        public int ReviewerId { get; set; }
    }
}
