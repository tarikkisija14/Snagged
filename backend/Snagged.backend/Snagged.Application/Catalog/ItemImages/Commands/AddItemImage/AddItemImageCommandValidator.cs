using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.AddItemImage
{
    public class AddItemImageCommandValidator : AbstractValidator<AddItemImageCommand>
    {
        public AddItemImageCommandValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0)
                .WithMessage("ItemId must be greater than 0.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage("ImageUrl is required.");
        }
    }
}
