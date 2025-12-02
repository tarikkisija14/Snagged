using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Subcategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Categories.Queries.GetCategories
{
    public class GetCategoriesHandler(IAppDbContext ctx):IRequestHandler<GetCategoriesQuery,List<CategoryWithSubcategoriesDto>>
    {
        public async Task<List<CategoryWithSubcategoriesDto>>Handle(GetCategoriesQuery request,CancellationToken ct)
        {
            return await ctx.Categories.Select(c => new CategoryWithSubcategoriesDto
            {
                Id = c.Id,
                Name = c.Name,
                Subcategories = c.Subcategories.Select(s => new SubcategoryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId,
                }).ToList(),

            }).ToListAsync(ct);
        }
    }
}
