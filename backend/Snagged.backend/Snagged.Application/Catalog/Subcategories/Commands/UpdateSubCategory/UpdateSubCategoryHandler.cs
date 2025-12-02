using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Commands.UpdateSubCategory
{
    public class UpdateSubCategoryHandler(IAppDbContext ctx): IRequestHandler<UpdateSubcategoryCommand, bool>
    {
        public async Task<bool> Handle(UpdateSubcategoryCommand request, CancellationToken ct)
        {
            var sub = await ctx.Subcategories.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (sub == null) return false;

            sub.Name = request.Name;
            sub.CategoryId = request.CategoryId;

            await ctx.SaveChangesAsync(ct);
            return true;
        }
    }
}
