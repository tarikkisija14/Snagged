using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Queries.GetCityById
{
    public class GetCityByIdQueryValidator : AbstractValidator<GetCityByIdQuery>
    {
        public GetCityByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
