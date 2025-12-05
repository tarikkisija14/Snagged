using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Queries.GetCountriesById
{
    public class GetCountriesByIdQueryValidator : AbstractValidator<GetCountriesByIdQuery>
    {
        public GetCountriesByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
