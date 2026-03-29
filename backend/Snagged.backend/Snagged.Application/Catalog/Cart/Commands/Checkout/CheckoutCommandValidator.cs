using FluentValidation;

namespace Snagged.Application.Catalog.Cart.Commands.Checkout
{
    public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
    {
        public CheckoutCommandValidator()
        {
           

            RuleFor(x => x.AddressId)
                .GreaterThan(0)
                .When(x => x.AddressId.HasValue)
                .WithMessage("AddressId must be greater than 0 when provided.");
        }
    }
}