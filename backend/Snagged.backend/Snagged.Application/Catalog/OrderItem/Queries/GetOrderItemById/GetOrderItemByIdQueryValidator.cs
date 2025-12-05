using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Queries.GetOrderItemById
{
    public class GetOrderItemByIdQueryValidator : AbstractValidator<GetOrderItemByIdQuery>
    {
        public GetOrderItemByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("OrderItem Id must be greater than 0.");
        }
    }
}
