using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Categories.Commands.AddCategory
{
    public class AddCategoryHandler(IAppDbContext ctx) : IRequestHandler<AddCategoryCommand,int>
    {
        public async Task<int>Handle(AddCategoryCommand request,CancellationToken ct)
        {
            var category = new Category
            {
                Name = request.Name
            };

            ctx.Categories.Add(category);
            await ctx.SaveChangesAsync();
            return category.Id;
        }
    }
}
