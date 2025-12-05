using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
    {
        public DeleteNotificationCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Notification Id must be greater than 0.");
        }
    }
}
