using MediatR;

namespace Snagged.Application.Catalog.Cart.Commands.MoveToCart
{
    public class MoveToCartCommand : IRequest<Unit>
    {
        public int CartId { get; set; }
    }
}