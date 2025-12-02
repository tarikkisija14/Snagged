using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler(IAppDbContext ctx) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken ct)
        {
            return await ctx.Categories
               .Where(c => c.Id == request.Id)
               .Select(c => new CategoryDto
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .FirstOrDefaultAsync(ct);
        }
    }
}
