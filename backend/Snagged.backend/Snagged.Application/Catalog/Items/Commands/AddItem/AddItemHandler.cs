using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Interfaces;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Items.Commands.AddItem
{
    public class AddItemHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<AddItemCommand, int>
    {
        public async Task<int> Handle(AddItemCommand request, CancellationToken ct)
        {
            var item = new Item
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Condition = request.Condition,
                CategoryId = request.CategoryId,
                SubcategoryId = request.SubcategoryId,
               
                UserId = currentUser.UserId,
                IsSold = false,
                CreatedAt = DateTime.UtcNow
            };

            for (int i = 0; i < request.ImageUrls.Count; i++)
            {
                item.Images.Add(new ItemImage
                {
                    ImageUrl = request.ImageUrls[i],
                    IsMain = i == 0
                });
            }

            ctx.Items.Add(item);
            await ctx.SaveChangesAsync(ct);

            return item.Id;
        }
    }
}