using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.AddCartItem
{
    public class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
    {
        public AddCartItemCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("ItemId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");
        }
    }
}
