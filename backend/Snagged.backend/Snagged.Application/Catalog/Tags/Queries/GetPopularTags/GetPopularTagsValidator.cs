using FluentValidation;

namespace Snagged.Application.Catalog.Tags.Queries.GetPopularTags
{
    public class GetPopularTagsValidator : AbstractValidator<GetPopularTagsQuery>
    {
        public GetPopularTagsValidator()
        {
            RuleFor(x => x.Limit)
                .GreaterThan(0).WithMessage("Limit must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Limit cannot exceed 50.");
        }
    }
}