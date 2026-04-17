using FluentValidation;

namespace Snagged.Application.Catalog.PushSubscriptions.Commands.Unsubscribe
{
    public class UnsubscribePushCommandValidator : AbstractValidator<UnsubscribePushCommand>
    {
        public UnsubscribePushCommandValidator()
        {
            RuleFor(x => x.Endpoint)
                .NotEmpty().WithMessage("Endpoint is required.");
        }
    }
}