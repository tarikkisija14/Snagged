using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewedUser
{
    public class GetReviewsByReviewedUserQueryValidator : AbstractValidator<GetReviewsByReviewedUserQuery>
    {
        public GetReviewsByReviewedUserQueryValidator()
        {
            RuleFor(x => x.ReviewedUserId)
                .GreaterThan(0).WithMessage("ReviewedUserId must be greater than 0.");
        }
    }
}
