using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Payment.Queries.GetAllPaymentsByUser
{
    public class GetPaymentsByUserQueryValidator : AbstractValidator<GetPaymentsByUserQuery>
    {
        public GetPaymentsByUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
