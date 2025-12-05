using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Queries.GetCartByUser
{
    public class GetCartByUserQueryValidator : AbstractValidator<GetCartByUserQuery>
    {
        public GetCartByUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
