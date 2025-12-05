using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Commands.AddAddress
{
    public class AddAddressCommandValidator :AbstractValidator<AddAddressCommand>
    {
        public AddAddressCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("CityId must be greater than 0.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(200).WithMessage("Street cannot exceed 200 characters.");

            RuleFor(x => x.Lat)
                .InclusiveBetween(-90, 90)
                .When(x => x.Lat.HasValue)
                .WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Lng)
                .InclusiveBetween(-180, 180)
                .When(x => x.Lng.HasValue)
                .WithMessage("Longitude must be between -180 and 180.");
        }
    }
}
