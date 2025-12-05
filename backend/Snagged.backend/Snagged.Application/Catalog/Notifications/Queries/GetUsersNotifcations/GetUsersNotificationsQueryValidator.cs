using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetUsersNotifcations
{
    public class GetUsersNotificationsQueryValidator : AbstractValidator<GetUsersNotificationsQuery>
    {
        public GetUsersNotificationsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
