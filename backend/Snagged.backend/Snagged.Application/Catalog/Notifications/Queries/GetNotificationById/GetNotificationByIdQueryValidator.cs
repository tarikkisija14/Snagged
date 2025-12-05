using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetNotificationById
{
    public class GetNotificationByIdQueryValidator : AbstractValidator<GetNotificationByIdQuery>
    {
        public GetNotificationByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Notification Id must be greater than 0.");
        }
    }
}
