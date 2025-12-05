using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Commands.CreateOrderItem
{
    public class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
    {
        public CreateOrderItemCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(x => x.Item)
                .NotNull().WithMessage("Item is required.");

            When(x => x.Item != null, () =>
            {
                RuleFor(x => x.Item.ItemId)
                    .GreaterThan(0).WithMessage("ItemId must be greater than 0.");

                RuleFor(x => x.Item.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be at least 1.");

                RuleFor(x => x.Item.Price)
                    .GreaterThanOrEqualTo(0).WithMessage("Price must be at least 0.");
            });
        }
    }
}
