using MediatR;

namespace Snagged.Application.Catalog.Review.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}