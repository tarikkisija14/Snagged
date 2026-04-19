using MediatR;
using Snagged.Application.Catalog.Items.Dto;

namespace Snagged.Application.Catalog.Items.Queries.GetItems
{
    public class GetItemsQuery : IRequest<List<ItemDto>>
    {
        public string? Search { get; set; }

        private int _take = 100;

        
        public int Take
        {
            get => _take;
            set => _take = value <= 0 ? 100 : value > 100 ? 100 : value;
        }
    }
}