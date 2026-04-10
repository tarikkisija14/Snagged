using MediatR;
using Snagged.Application.Catalog.Items.Dto;

namespace Snagged.Application.Catalog.Items.Queries.GetItems
{
    public class GetItemsQuery : IRequest<List<ItemDto>>
    {
        public string? Search { get; set; }

      
        public int Take { get; set; } = 100;
    }
}