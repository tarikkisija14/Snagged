using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryHandler(IAppDbContext ctx) : IRequestHandler<DeleteCategoryCommand,bool>
    {
        public async Task<bool>Handle(DeleteCategoryCommand request,CancellationToken ct)
        {
            var cat = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if(cat == null)
                return false;
            ctx.Categories.Remove(cat);
            await ctx.SaveChangesAsync();
            return true;
        }
    }
}
