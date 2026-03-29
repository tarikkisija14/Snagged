using MediatR;

namespace Snagged.Application.Catalog.Review.Commands.AddReview
{
    public class AddReviewCommand : IRequest<int>
    {
        public int ReviewedUserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}