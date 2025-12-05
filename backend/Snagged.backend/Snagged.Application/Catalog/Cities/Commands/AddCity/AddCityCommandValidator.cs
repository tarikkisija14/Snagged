using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Commands.AddCity
{
    public class AddCityCommandValidator : AbstractValidator<AddCityCommand>
    {
        public AddCityCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("City name is required.")
                .MaximumLength(100).WithMessage("City name cannot exceed 100 characters.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("CountryId must be greater than 0.");
        }
    }
}
