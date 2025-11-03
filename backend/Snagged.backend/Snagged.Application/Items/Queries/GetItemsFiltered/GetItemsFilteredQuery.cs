using Snagged.Application.Commom.Paging;
using Snagged.Application.Items.Queries.GetItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Items.Queries.GetItemsFiltered
{
    public class GetItemsFilteredQuery: BasePagedQuery<ItemDto>
    {
        public int? UserId { get; init; }
        public int? CategoryId { get; init; }
        public int? SubcategoryId { get; init; }
        public string? TitleContains { get; init; }
        public string? Condition { get; init; }
        public bool? IsSold { get; init; }
    }
}
