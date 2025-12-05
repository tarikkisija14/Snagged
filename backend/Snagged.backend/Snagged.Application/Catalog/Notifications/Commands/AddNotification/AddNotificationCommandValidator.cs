using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Commands.AddNotification
{
    public class AddNotificationCommandValidator : AbstractValidator<AddNotificationCommand>
    {
        public AddNotificationCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.");

            RuleFor(x => x.NotificationType)
                .NotEmpty().WithMessage("NotificationType is required.");
        }
    }
}
