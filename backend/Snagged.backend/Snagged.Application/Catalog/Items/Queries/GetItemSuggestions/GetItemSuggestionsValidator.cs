
using FluentValidation;

namespace Snagged.Application.Catalog.Items.Queries.GetItemSuggestions
{
    public class GetItemSuggestionsValidator : AbstractValidator<GetItemSuggestionsQuery>
    {
        public GetItemSuggestionsValidator()
        {
            RuleFor(x => x.Query)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100);

            RuleFor(x => x.Limit)
                .InclusiveBetween(1, 20);
        }
    }
}