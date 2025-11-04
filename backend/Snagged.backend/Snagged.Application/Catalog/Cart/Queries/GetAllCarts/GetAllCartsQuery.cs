using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Queries.GetAllCarts
{
    public class GetAllCartsQuery:IRequest<List<CartDto>>
    {

    }
}
