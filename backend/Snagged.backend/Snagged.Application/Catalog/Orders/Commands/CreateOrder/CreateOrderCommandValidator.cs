using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Order)
                .NotNull().WithMessage("Order is required.");

            When(x => x.Order != null, () =>
            {
                RuleFor(x => x.Order.BuyerId)
                    .GreaterThan(0).WithMessage("BuyerId must be greater than 0.");

                RuleFor(x => x.Order.Status)
                    .NotEmpty().WithMessage("Status is required.");

                RuleForEach(x => x.Order.Items).SetValidator(new OrdersCreateOrderItemDtoValidator());
            });
        }
    }
}
