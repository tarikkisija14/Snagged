using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.DeleteItemImage
{
    public class DeleteItemImageCommandValidator : AbstractValidator<DeleteItemImageCommand>
    {
        public DeleteItemImageCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Image Id must be greater than 0.");
        }
    }
}
