using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Queries.GetOrderItemsByOrderId
{
    public class GetOrderItemsByOrderIdQueryValidator : AbstractValidator<GetOrderItemsByOrderIdQuery>
    {
        public GetOrderItemsByOrderIdQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");
        }
    }
}
