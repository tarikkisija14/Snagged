using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Application.Common.Paging;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Items.Queries.GetItemsFiltered
{
    public class GetItemsFilteredHandler(IAppDbContext ctx)
        : IRequestHandler<GetItemsFilteredQuery, PageResult<ItemDto>>
    {
        public async Task<PageResult<ItemDto>> Handle(GetItemsFilteredQuery request, CancellationToken ct)
        {
            var query = ctx.Items
                .AsNoTracking()
                .IncludeItemDetails();

            query = ApplyFilters(query, request);

            var sorted = ApplySorting(query, request);
            var dtoQuery = sorted.ProjectToDto();
            var paging = ResolvePaging(request);

            return await PageResult<ItemDto>.FromQueryableAsync(dtoQuery, paging, ct);
        }

        private static IQueryable<Item> ApplyFilters(IQueryable<Item> query, GetItemsFilteredQuery request)
        {
            
            if (request.IsSold.HasValue)
                query = query.Where(x => x.IsSold == request.IsSold.Value);
            else
                query = query.Where(x => !x.IsSold);

            if (request.CategoryIds is { Count: > 0 })
                query = query.Where(x => request.CategoryIds.Contains(x.CategoryId));

            if (request.SubcategoryIds is { Count: > 0 })
                query = query.Where(x => x.SubcategoryId.HasValue &&
                                         request.SubcategoryIds.Contains(x.SubcategoryId.Value));

            if (request.Conditions is { Count: > 0 })
                query = query.Where(x => request.Conditions.Contains(x.Condition));

            if (!string.IsNullOrWhiteSpace(request.TitleContains))
                query = query.Where(x => x.Title.Contains(request.TitleContains));

            if (request.MinPrice.HasValue)
                query = query.Where(x => x.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(x => x.Price <= request.MaxPrice.Value);

            return query;
        }

        private static IOrderedQueryable<Item> ApplySorting(IQueryable<Item> query, GetItemsFilteredQuery request)
        {
            return request.SortBy?.ToLowerInvariant() switch
            {
                "price" => request.SortOrder?.ToLowerInvariant() == "desc"
                    ? query.OrderByDescending(x => x.Price)
                    : query.OrderBy(x => x.Price),

                "popularity" => query.OrderByDescending(x => x.Favorites.Count),

                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }

        private static PageRequest ResolvePaging(GetItemsFilteredQuery request)
        {
            if (request.LoadAllItems == true)
                return new PageRequest { Page = 1, PageSize = int.MaxValue };

            return request.Paging ?? new PageRequest { Page = 1, PageSize = 12 };
        }
    }
}