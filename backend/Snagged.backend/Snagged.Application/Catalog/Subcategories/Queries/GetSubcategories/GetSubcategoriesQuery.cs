using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Queries.GetSubcategories
{
    public class GetSubcategoriesQuery:IRequest<List<SubcategoryDto>>
    {
        public int? CategoryId { get; set; }
    }
}
