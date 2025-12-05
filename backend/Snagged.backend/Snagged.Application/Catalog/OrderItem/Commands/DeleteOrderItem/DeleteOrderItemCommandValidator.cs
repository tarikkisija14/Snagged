using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.OrderItem.Commands.DeleteOrderItem
{
    public class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
    {
        public DeleteOrderItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("OrderItem Id must be greater than 0.");
        }
    }
}
