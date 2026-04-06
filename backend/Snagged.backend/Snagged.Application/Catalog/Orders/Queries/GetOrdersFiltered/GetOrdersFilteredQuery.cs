using Snagged.Application.Catalog.Orders.Commands;
using Snagged.Application.Common.Paging;

namespace Snagged.Application.Catalog.Orders.Queries.GetOrdersFiltered
{
    public class GetOrdersFilteredQuery : BasePagedQuery<OrderDto>
    {
        public int? BuyerId { get; set; }
        public string? Status { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public decimal? MinTotalAmount { get; set; }
        public decimal? MaxTotalAmount { get; set; }
        public int? PaymentId { get; set; }
        public bool? IsPaid { get; set; }
    }
}