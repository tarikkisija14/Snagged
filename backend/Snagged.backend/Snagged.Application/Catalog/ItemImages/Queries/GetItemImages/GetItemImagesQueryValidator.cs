using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Queries.GetItemImages
{
    public class GetItemImagesQueryValidator : AbstractValidator<GetItemImagesQuery>
    {
        public GetItemImagesQueryValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0)
                .WithMessage("ItemId must be greater than 0.");
        }
    }
}
