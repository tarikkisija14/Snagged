using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.DeleteAllItemImages
{
    public class DeleteAllItemImagesCommandValidator : AbstractValidator<DeleteAllItemImagesCommand>
    {
        public DeleteAllItemImagesCommandValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0)
                .WithMessage("ItemId must be greater than 0.");
        }
    }
}
