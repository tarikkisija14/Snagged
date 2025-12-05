using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Queries.GetOrdersById
{
    public class GetOrdersByIdQueryValidator : AbstractValidator<GetOrdersByIdQuery>
    {
        public GetOrdersByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Order Id must be greater than 0.");
        }
    }
}
