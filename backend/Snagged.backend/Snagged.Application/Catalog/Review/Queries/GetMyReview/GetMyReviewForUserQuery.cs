using MediatR;

namespace Snagged.Application.Catalog.Review.Queries.GetMyReview
{
   
    public class GetMyReviewForUserQuery : IRequest<ReviewDto?>
    {
        public int ReviewedUserId { get; set; }
    }
}