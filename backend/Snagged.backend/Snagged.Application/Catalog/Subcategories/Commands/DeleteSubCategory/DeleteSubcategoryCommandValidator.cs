using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Commands.DeleteSubCategory
{
    public class DeleteSubcategoryCommandValidator : AbstractValidator<DeleteSubcategoryCommand>
    {
        public DeleteSubcategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Subcategory Id must be greater than 0.");
        }
    }
}
