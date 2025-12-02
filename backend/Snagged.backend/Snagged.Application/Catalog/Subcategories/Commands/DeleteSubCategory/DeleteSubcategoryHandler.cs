using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Commands.DeleteSubCategory
{
    public class DeleteSubcategoryHandler(IAppDbContext ctx): IRequestHandler<DeleteSubcategoryCommand, bool>

    {
        public async Task<bool> Handle(DeleteSubcategoryCommand request, CancellationToken ct)
        {
            var sub = await ctx.Subcategories.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (sub == null) return false;

            var hasItems = await ctx.Items.AnyAsync(i => i.SubcategoryId == request.Id, ct);
            if (hasItems)
                throw new InvalidOperationException("Cannot delete subcategory because it has items.");


            ctx.Subcategories.Remove(sub);
            await ctx.SaveChangesAsync(ct);

            return true;
        }
    }
}
