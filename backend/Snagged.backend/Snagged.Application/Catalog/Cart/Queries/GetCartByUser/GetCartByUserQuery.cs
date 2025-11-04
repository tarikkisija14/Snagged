using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Queries.GetCartByUser
{
    public class GetCartByUserQuery : IRequest<CartDto>
    {
        public int UserId { get; set; }
    }
}
