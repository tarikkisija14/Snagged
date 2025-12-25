// Snagged.Application.Catalog.Items.Queries.GetItemsFiltered/GetItemsFilteredHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Paging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Snagged.Application.Catalog.Items.Queries.GetItems; // Dodano za ItemDto

namespace Snagged.Application.Catalog.Items.Queries.GetItemsFiltered
{
    public class GetItemsFilteredHandler(IAppDbContext ctx) : IRequestHandler<GetItemsFilteredQuery, PageResult<ItemDto>>
    {
        public async Task<PageResult<ItemDto>> Handle(GetItemsFilteredQuery request, CancellationToken ct)
        {
            var query = ctx.Items
               .AsNoTracking()
               .Include(x => x.User)
               .Include(x => x.Category)
               .Include(x => x.Subcategory)
               .Include(x => x.Images)
               .AsQueryable();

           

            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                query = query.Where(x => request.CategoryIds.Contains(x.CategoryId));
            }

            if (request.SubcategoryIds != null && request.SubcategoryIds.Any())
            {
                query = query.Where(x => x.SubcategoryId.HasValue &&
                    request.SubcategoryIds.Contains(x.SubcategoryId.Value));
            }

            if (request.Conditions != null && request.Conditions.Any())
            {
                query = query.Where(x => request.Conditions.Contains(x.Condition));
            }

            if (!string.IsNullOrWhiteSpace(request.TitleContains))
            {
                query = query.Where(x => x.Title.Contains(request.TitleContains));
            }

            if (request.IsSold.HasValue)
            {
                query = query.Where(x => x.IsSold == request.IsSold.Value);
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(x => x.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= request.MaxPrice.Value);
            }

            

            IOrderedQueryable<Domain.Entities.Item> orderedQuery;

            switch (request.SortBy?.ToLower())
            {
                case "price":
                    orderedQuery = request.SortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(x => x.Price)
                        : query.OrderBy(x => x.Price);
                    break;

                case "createdat":
                case "newest":
                    orderedQuery = query.OrderByDescending(x => x.CreatedAt);
                    break;

                case "popularity":
                    orderedQuery = query.OrderByDescending(x => x.Favorites.Count);
                    break;

                default:
                    orderedQuery = query.OrderByDescending(x => x.CreatedAt);
                    break;
            }

           
            var dtoQuery = orderedQuery.Select(x => new ItemDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                Condition = x.Condition,
                IsSold = x.IsSold,
                CategoryName = x.Category.Name,
                SubcategoryName = x.Subcategory != null ? x.Subcategory.Name : null,
                SellerUsername = x.User.Email,
                ImageUrls = x.Images.Select(i => i.ImageUrl).ToList()
            }).AsQueryable();

            

            var paging = request.Paging ?? new PageRequest { Page = 1, PageSize = 12 };

            
            if (request.LoadAllItems.HasValue && request.LoadAllItems.Value)
            {
              
                paging = new PageRequest { Page = 1, PageSize = int.MaxValue };
            }

           
            return await PageResult<ItemDto>.FromQueryableAsync(dtoQuery, paging, ct);
        }
    }
}