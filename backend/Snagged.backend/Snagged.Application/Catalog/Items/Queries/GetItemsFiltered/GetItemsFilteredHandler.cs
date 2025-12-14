using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Queries.GetItems;
using Snagged.Application.Commom.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Kategorije - sada podržava array
            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                query = query.Where(x => request.CategoryIds.Contains(x.CategoryId));
            }

            // Podkategorije - sada podržava array
            if (request.SubcategoryIds != null && request.SubcategoryIds.Any())
            {
                query = query.Where(x => x.SubcategoryId.HasValue &&
                    request.SubcategoryIds.Contains(x.SubcategoryId.Value));
            }

            // Uslovi - sada podržava array
            if (request.Conditions != null && request.Conditions.Any())
            {
                query = query.Where(x => request.Conditions.Contains(x.Condition));
            }

            // Pretraga po naslovu
            if (!string.IsNullOrWhiteSpace(request.TitleContains))
            {
                query = query.Where(x => x.Title.Contains(request.TitleContains));
            }

            // Prodato/ne prodato
            if (request.IsSold.HasValue)
            {
                query = query.Where(x => x.IsSold == request.IsSold.Value);
            }

            // Cijena - range
            if (request.MinPrice.HasValue)
            {
                query = query.Where(x => x.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= request.MaxPrice.Value);
            }

           

            switch (request.SortBy?.ToLower())
            {
                case "price":
                    query = request.SortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(x => x.Price)
                        : query.OrderBy(x => x.Price);
                    break;

                case "createdat":
                case "newest":
                    query = query.OrderByDescending(x => x.CreatedAt);
                    break;

                case "popularity":
                    query = query.OrderByDescending(x => x.Favorites.Count);
                    break;

                default:
                    query = query.OrderByDescending(x => x.CreatedAt);
                    break;
            }

            var items = await query.ToListAsync(ct);

            var dtoQuery = query.Select(x => new ItemDto
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

            return await PageResult<ItemDto>.FromQueryableAsync(dtoQuery, request.Paging, ct);
        }
    }
}
