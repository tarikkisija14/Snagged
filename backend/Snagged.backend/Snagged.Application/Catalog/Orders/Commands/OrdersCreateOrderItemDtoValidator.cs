using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Commands
{
    public class OrdersCreateOrderItemDtoValidator : AbstractValidator<OrdersCreateOrderItemDto>
    {
        public OrdersCreateOrderItemDtoValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("ItemId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be at least 0.");
        }
    }
}
