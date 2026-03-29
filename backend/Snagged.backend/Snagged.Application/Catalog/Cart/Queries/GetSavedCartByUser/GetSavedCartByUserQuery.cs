using MediatR;
using Snagged.Application.Catalog.Cart;

namespace Snagged.Application.Catalog.Cart.Queries.GetSavedCartByUser
{
    public class GetSavedCartByUserQuery : IRequest<CartDto?>
    {
    }
}