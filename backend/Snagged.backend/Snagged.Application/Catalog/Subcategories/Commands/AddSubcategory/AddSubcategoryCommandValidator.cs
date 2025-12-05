using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Commands.AddSubcategory
{
    public class AddSubcategoryCommandValidator : AbstractValidator<AddSubcategoryCommand>
    {
        public AddSubcategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Subcategory name is required.")
                .MaximumLength(100).WithMessage("Subcategory name must be at most 100 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");
        }
    }
}
