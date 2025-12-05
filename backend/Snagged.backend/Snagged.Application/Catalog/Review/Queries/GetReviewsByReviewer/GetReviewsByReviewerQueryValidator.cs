using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewer
{
    public class GetReviewsByReviewerQueryValidator : AbstractValidator<GetReviewsByReviewerQuery>
    {
        public GetReviewsByReviewerQueryValidator()
        {
            RuleFor(x => x.ReviewerId)
                .GreaterThan(0).WithMessage("ReviewerId must be greater than 0.");
        }
    }
}
