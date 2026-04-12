using FluentValidation;

namespace Snagged.Application.Catalog.Tags.Queries.GetTagSuggestions
{
    public class GetTagSuggestionsValidator : AbstractValidator<GetTagSuggestionsQuery>
    {
        public GetTagSuggestionsValidator()
        {
            RuleFor(x => x.Limit)
                .GreaterThan(0).WithMessage("Limit must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Limit cannot exceed 50.");
        }
    }
}