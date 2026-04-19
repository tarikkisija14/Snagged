using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Items.Commands.UpdateItem
{
    public class UpdateItemHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<UpdateItemCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken ct)
        {
            var item = await ctx.Items.FindAsync(new object[] { request.Id }, ct);

            if (item is null)
                throw new SnaggedNotFoundException($"Item with id {request.Id} was not found.");

            if (item.UserId != currentUser.UserId)
                throw new UnauthorizedAccessException("You can only update your own items.");

            item.Title = request.Title;
            item.Description = request.Description;
            item.Price = request.Price;
            item.Condition = request.Condition;
            item.IsSold = request.IsSold;
            item.CategoryId = request.CategoryId;
            item.SubcategoryId = request.SubcategoryId;

            await SyncTagsAsync(ctx, item, request.Tags, ct);

            await ctx.SaveChangesAsync(ct);
            return Unit.Value;
        }

        private static async Task SyncTagsAsync(
            IAppDbContext ctx, Item item, List<string> tagNames, CancellationToken ct)
        {
            var existing = await ctx.ItemTags
                .Where(it => it.ItemId == item.Id)
                .Include(it => it.Tag)
                .ToListAsync(ct);

            var desired = tagNames
                .Select(t => t.Trim().ToLowerInvariant())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToHashSet();

            var toRemove = existing.Where(it => !desired.Contains(it.Tag.Name)).ToList();
            ctx.ItemTags.RemoveRange(toRemove);

            var currentNames = existing
                .Where(it => desired.Contains(it.Tag.Name))
                .Select(it => it.Tag.Name)
                .ToHashSet();

            foreach (var name in desired.Except(currentNames))
            {
                var tag = await ctx.Tags.FirstOrDefaultAsync(t => t.Name == name, ct)
                          ?? new Tag { Name = name };

                if (tag.Id == 0)
                    ctx.Tags.Add(tag);

                ctx.ItemTags.Add(new ItemTag { Item = item, Tag = tag });
            }
        }
    }
}