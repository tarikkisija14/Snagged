using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Application.Catalog.Items.Queries.GetMyItems;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Items.Queries.GetMyItemsHandler
{
    public class GetMyItemsQueryHandler
        : IRequestHandler<GetMyItemsQuery, List<ItemDto>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetMyItemsQueryHandler(
            IAppDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<List<ItemDto>> Handle(
            GetMyItemsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            return await _context.Items
                .AsNoTracking()
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Price = i.Price,
                    Condition = i.Condition,
                    IsSold = i.IsSold,
                    CreatedAt = i.CreatedAt,
                    CategoryId = i.CategoryId,
                    LikesCount = _context.Favorites.Count(f=> f.ItemId == i.Id),
                    SubcategoryId = i.SubcategoryId,

                    Images = i.Images
                        .OrderByDescending(img => img.IsMain)
                        .ThenBy(img => img.Id)
                        .Select(img => new ItemImageDto
                        {
                            Id = img.Id,
                            ImageUrl = img.ImageUrl,
                            IsMain = img.IsMain
                        })
                        .ToList()

                })
                .ToListAsync(cancellationToken);
        }
    }
}
