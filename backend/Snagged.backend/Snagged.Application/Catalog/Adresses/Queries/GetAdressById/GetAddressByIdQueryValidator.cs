using FluentValidation;
using Snagged.Application.Catalog.Adresses.Queries.GetAdressesById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Queries.GetAdressById
{
    public class GetAddressByIdQueryValidator : AbstractValidator<GetAddressByIdQuery>
    {
        public GetAddressByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
