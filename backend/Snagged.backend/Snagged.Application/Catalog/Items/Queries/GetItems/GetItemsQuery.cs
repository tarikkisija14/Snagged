using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Queries.GetItems
{
    public class GetItemsQuery :IRequest<List<ItemDto>>
    {
        public string? Search { get; set; }
    }
}
