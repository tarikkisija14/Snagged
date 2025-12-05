using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Notifications.Queries.GetUnreadCountForUser
{
    public class GetUnreadCountQueryValidator : AbstractValidator<GetUnreadCountQuery>
    {
        public GetUnreadCountQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
