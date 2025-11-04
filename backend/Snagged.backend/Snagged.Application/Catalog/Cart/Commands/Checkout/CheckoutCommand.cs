using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.Checkout
{
    public class CheckoutCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int? AddressId { get; set; } 
    }
}
