using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Queries.GetOrdersFiltered
{
    public class GetOrdersFilteredQueryValidator : AbstractValidator<GetOrdersFilteredQuery>
    {
        public GetOrdersFilteredQueryValidator()
        {
            RuleFor(x => x.MinTotalAmount)
                .GreaterThanOrEqualTo(0).When(x => x.MinTotalAmount.HasValue)
                .WithMessage("MinTotalAmount cannot be negative.");

            RuleFor(x => x.MaxTotalAmount)
                .GreaterThanOrEqualTo(0).When(x => x.MaxTotalAmount.HasValue)
                .WithMessage("MaxTotalAmount cannot be negative.");

            RuleFor(x => x)
                .Must(x => !x.MinTotalAmount.HasValue || !x.MaxTotalAmount.HasValue || x.MinTotalAmount <= x.MaxTotalAmount)
                .WithMessage("MinTotalAmount cannot be greater than MaxTotalAmount.");

            RuleFor(x => x.OrderDateFrom)
                .LessThanOrEqualTo(x => x.OrderDateTo).When(x => x.OrderDateFrom.HasValue && x.OrderDateTo.HasValue)
                .WithMessage("OrderDateFrom cannot be after OrderDateTo.");
        }
    }

}
