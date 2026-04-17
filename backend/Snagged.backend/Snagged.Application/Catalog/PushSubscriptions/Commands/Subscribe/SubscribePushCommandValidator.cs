using FluentValidation;

namespace Snagged.Application.Catalog.PushSubscriptions.Commands.Subscribe
{
    public class SubscribePushCommandValidator : AbstractValidator<SubscribePushCommand>
    {
        public SubscribePushCommandValidator()
        {
            RuleFor(x => x.Endpoint)
                .NotEmpty().WithMessage("Endpoint is required.")
                .Must(e => e.StartsWith("https://")).WithMessage("Endpoint must be a valid HTTPS URL.");

            RuleFor(x => x.P256DhKey)
                .NotEmpty().WithMessage("P256DhKey is required.");

            RuleFor(x => x.AuthKey)
                .NotEmpty().WithMessage("AuthKey is required.");
        }
    }
}