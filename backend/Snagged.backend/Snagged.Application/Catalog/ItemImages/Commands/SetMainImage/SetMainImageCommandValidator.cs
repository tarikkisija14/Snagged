using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.SetMainImage
{
    public class SetMainImageCommandValidator : AbstractValidator<SetMainImageCommand>
    {
        public SetMainImageCommandValidator()
        {
            RuleFor(x => x.ImageId)
                .GreaterThan(0)
                .WithMessage("ImageId must be greater than 0.");
        }
    }
}
