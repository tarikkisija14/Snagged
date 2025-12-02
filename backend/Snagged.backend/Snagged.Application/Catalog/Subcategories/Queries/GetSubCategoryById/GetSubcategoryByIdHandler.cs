using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Subcategories.Queries.GetSubCategoriesById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Queries.GetSubCategoryById
{
    public class GetSubCategoryByIdHandler(IAppDbContext ctx) : IRequestHandler<GetSubcategoryByIdQuery, SubcategoryDto>
    {
        public async Task<SubcategoryDto> Handle(GetSubcategoryByIdQuery request, CancellationToken ct)
        {
            return await ctx.Subcategories
              .Where(s => s.Id == request.Id)
              .Select(s => new SubcategoryDto
              {
                  Id = s.Id,
                  Name = s.Name,
                  CategoryId = s.CategoryId
              })
              .FirstOrDefaultAsync(ct);
        }
    }
}
