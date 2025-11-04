using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Commands.UpdateItem
{
    public class UpdateItemHandler(IAppDbContext ctx) : IRequestHandler<UpdateItemCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken ct)
        {
            var item = await ctx.Items
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(ct);

            if (item == null)
                throw new KeyNotFoundException($"Item with id {request.Id} not found.");

            
            item.Title = request.Title;
            item.Description = request.Description;
            item.Price = request.Price;
            item.Condition = request.Condition;
            item.IsSold = request.IsSold;
            item.CategoryId = request.CategoryId;
            item.SubcategoryId = request.SubcategoryId;

            await ctx.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }

}
