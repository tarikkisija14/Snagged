using MediatR;

namespace Snagged.Application.Catalog.Review.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}