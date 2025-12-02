using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Queries.GetSubcategories
{
    public class GetSubcategoriesHandler(IAppDbContext ctx) : IRequestHandler<GetSubcategoriesQuery, List<SubcategoryDto>>
    {
        public async Task<List<SubcategoryDto>> Handle(GetSubcategoriesQuery request, CancellationToken ct)
        {
            var q = ctx.Subcategories.AsQueryable();

            if (request.CategoryId.HasValue)
                q = q.Where(s => s.CategoryId == request.CategoryId.Value);

            return await q
                .Select(s => new SubcategoryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId
                })
                .ToListAsync(ct);
        }
    }
}
