using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.ClearCart
{
    public class ClearCartCommand:IRequest<Unit>
    {
        public int UserId { get; set; }
    }
}
