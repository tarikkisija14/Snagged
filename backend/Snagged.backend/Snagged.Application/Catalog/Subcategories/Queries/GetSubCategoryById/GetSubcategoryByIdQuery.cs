using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Queries.GetSubCategoriesById
{
    public class GetSubcategoryByIdQuery : IRequest<SubcategoryDto>
    {
        public int Id { get; set; }
        public GetSubcategoryByIdQuery(int id) => Id = id;
    }
}
