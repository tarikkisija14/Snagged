using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Items.Commands.UpdateItem
{
    public class UpdateItemHandler(IAppDbContext ctx) : IRequestHandler<UpdateItemCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken ct)
        {
            var item = await ctx.Items.FindAsync(new object[] { request.Id }, ct);

            if (item is null)
                throw new SnaggedNotFoundException($"Item with id {request.Id} was not found.");

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