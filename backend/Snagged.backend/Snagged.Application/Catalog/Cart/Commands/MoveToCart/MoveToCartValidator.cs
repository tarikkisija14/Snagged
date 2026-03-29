using FluentValidation;

namespace Snagged.Application.Catalog.Cart.Commands.MoveToCart
{
    public class MoveToCartValidator : AbstractValidator<MoveToCartCommand>
    {
        public MoveToCartValidator()
        {
            RuleFor(x => x.CartId)
                .GreaterThan(0).WithMessage("CartId must be greater than 0.");
        }
    }
}