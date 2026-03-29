using FluentValidation;

namespace Snagged.Application.Catalog.Payment.Commands.CreateStripePayment
{
    public class CreateStripePaymentCommandValidator : AbstractValidator<CreateStripePaymentCommand>
    {
        
        public CreateStripePaymentCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Length(3).WithMessage("Currency must be a 3-letter ISO code (e.g. 'usd').");
        }
    }
}