using FluentValidation;

namespace Snagged.Application.Catalog.PushSubscriptions.Queries.GetPushSubscriptionStatus
{
    public class GetPushSubscriptionStatusQueryValidator : AbstractValidator<GetPushSubscriptionStatusQuery>
    {
        public GetPushSubscriptionStatusQueryValidator()
        {
            RuleFor(x => x.Endpoint)
                .NotEmpty().WithMessage("Endpoint is required.");
        }
    }
}