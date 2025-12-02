using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler(IAppDbContext ctx) : IRequestHandler<UpdateCategoryCommand,bool>
    {
        public async Task<bool> Handle(UpdateCategoryCommand request,CancellationToken ct)
        {
            var cat= await ctx.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if(cat==null)
                return false;

            cat.Name= request.Name; 
            await ctx.SaveChangesAsync(ct);
            return true;
        }
    }
}
