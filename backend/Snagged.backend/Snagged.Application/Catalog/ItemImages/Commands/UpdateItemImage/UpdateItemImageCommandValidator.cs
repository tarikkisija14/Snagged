using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.UpdateItemImage
{
    public class UpdateItemImageCommandValidator : AbstractValidator<UpdateItemImageCommand>
    {
        public UpdateItemImageCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Image Id must be greater than 0.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage("ImageUrl is required.");
        }
    }
}
