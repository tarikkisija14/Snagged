using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.SaveForLater
{
    public  class SaveForLaterCommandValidator : AbstractValidator<SaveForLaterCommand>
    {
        public SaveForLaterCommandValidator()
        {
            RuleFor(x => x.CartId)
                .GreaterThan(0).WithMessage("CartId must be greater than 0.");
        }
    }
}
