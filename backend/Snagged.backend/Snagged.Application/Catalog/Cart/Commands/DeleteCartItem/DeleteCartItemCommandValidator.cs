using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.DeleteCartItem
{
    public class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand>
    {
        public DeleteCartItemCommandValidator()
        {
            RuleFor(x => x.CartItemId)
                .GreaterThan(0).WithMessage("CartItemId must be greater than 0.");
        }
    }
}
