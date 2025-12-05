using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommandValidator : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("OrderItem Id must be greater than 0.");

            RuleFor(x => x.Item)
                .NotNull().WithMessage("Item is required.");

            When(x => x.Item != null, () =>
            {
                RuleFor(x => x.Item.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be at least 1.");

                RuleFor(x => x.Item.Price)
                    .GreaterThanOrEqualTo(0).WithMessage("Price must be at least 0.");
            });
        }
    }
}
