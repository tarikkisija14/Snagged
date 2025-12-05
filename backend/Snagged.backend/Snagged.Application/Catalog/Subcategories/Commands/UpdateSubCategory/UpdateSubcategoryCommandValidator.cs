using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Commands.UpdateSubCategory
{
    public class UpdateSubcategoryCommandValidator : AbstractValidator<UpdateSubcategoryCommand>
    {
        public UpdateSubcategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Subcategory Id must be greater than 0.");

            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Subcategory name is required.")
                    .MaximumLength(100).WithMessage("Subcategory name must be at most 100 characters.");

            RuleFor(x => x.CategoryId)
                    .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");
        }
    }
}
