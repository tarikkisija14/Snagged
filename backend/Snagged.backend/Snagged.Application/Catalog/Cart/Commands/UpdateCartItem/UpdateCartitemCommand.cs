using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<Unit>
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
