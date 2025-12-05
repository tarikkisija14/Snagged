using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.MarkAsRead
{
    public class MarkAsReadCommandValidator : AbstractValidator<MarkAsReadCommand>
    {
        public MarkAsReadCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Notification Id must be greater than 0.");
        }
    }
}
