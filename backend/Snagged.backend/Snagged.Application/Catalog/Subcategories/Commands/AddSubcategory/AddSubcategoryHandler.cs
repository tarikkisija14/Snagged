using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Commands.AddSubcategory
{
    public class AddSubcategoryHandler(IAppDbContext ctx): IRequestHandler<AddSubcategoryCommand,int>
    {
        public async Task<int>Handle(AddSubcategoryCommand request,CancellationToken ct)
        {
            var sub = new Subcategory
            {
                Name = request.Name,
                CategoryId = request.CategoryId
            };

           ctx.Subcategories.Add(sub);
            await ctx.SaveChangesAsync();
            return sub.Id;
        }
    }
}
