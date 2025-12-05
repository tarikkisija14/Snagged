using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.Checkout
{
    public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
    {
        public CheckoutCommandValidator()
        {
            RuleFor(x => x.UserId)
               .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.AddressId)
                .GreaterThan(0)
                .When(x => x.AddressId.HasValue)
                .WithMessage("AddressId must be greater than 0 when provided.");
        }
    }
}
