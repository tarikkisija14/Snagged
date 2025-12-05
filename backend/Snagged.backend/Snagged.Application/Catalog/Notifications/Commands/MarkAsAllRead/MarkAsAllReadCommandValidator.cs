using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.MarkAsAllRead
{
    public class MarkAsAllReadCommandValidator : AbstractValidator<MarkAsAllReadCommand>
    {
        public MarkAsAllReadCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
