using FluentValidation;
using Snagged.Application.Catalog.Subcategories.Queries.GetSubCategoriesById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Queries.GetSubCategoryById
{
    public class GetSubcategoryByIdValidator : AbstractValidator<GetSubcategoryByIdQuery>
    {
        public GetSubcategoryByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Subcategory Id must be greater than 0.");
        }
    }
}
