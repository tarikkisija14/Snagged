using Snagged.Application.Catalog.Orders.Commands;
using Snagged.Application.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Orders.Queries.GetOrdersPaged
{
    public class GetOrdersPagedQuery : BasePagedQuery<OrderDto>
    {
        public int? BuyerId { get; set; }   
        public string? Status {  get; set; } 

    }
}
