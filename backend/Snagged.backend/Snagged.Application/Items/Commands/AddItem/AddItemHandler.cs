using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Items.Commands.AddItem
{
    public class AddItemHandler(IAppDbContext ctx) :IRequestHandler<AddItemCommand,int>
    {
        public async Task<int>Handle(AddItemCommand request,CancellationToken ct)
        {
            var item = new Item
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Condition = request.Condition,
                CategoryId = request.CategoryId,
                SubcategoryId = request.SubcategoryId,
                UserId = request.UserId,
                IsSold = false,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var url in request.ImageUrls)
            {
                item.Images.Add(new ItemImage { ImageUrl = url });
            }
            
            ctx.Items.Add(item);
            await ctx.SaveChangesAsync(ct);

            return item.Id;

        }
    }
}
