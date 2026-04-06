using FluentValidation;

namespace Snagged.Application.Catalog.Review.Commands.AddReview
{
    public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewCommandValidator()
        {
            RuleFor(x => x.ReviewedUserId)
                .GreaterThan(0).WithMessage("ReviewedUserId must be greater than 0.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MinimumLength(3).WithMessage("Comment must be at least 3 characters.")
                .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
        }
    }
}