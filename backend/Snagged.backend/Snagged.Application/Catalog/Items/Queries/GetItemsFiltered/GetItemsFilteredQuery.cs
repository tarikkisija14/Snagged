using Snagged.Application.Catalog.Items.Queries.GetItems;
using Snagged.Application.Commom.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Queries.GetItemsFiltered
{
    public class GetItemsFilteredQuery: BasePagedQuery<ItemDto>
    {
        public List<int>? CategoryIds { get; init; } 
        public List<int>? SubcategoryIds { get; init; } 
        public List<string>? Conditions { get; init; } 
        public string? TitleContains { get; init; }
        public bool? IsSold { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }

        public bool? LoadAllItems { get; set; }

        public string? SortBy { get; init; }
        public string? SortOrder { get; init; } 
    }
}
