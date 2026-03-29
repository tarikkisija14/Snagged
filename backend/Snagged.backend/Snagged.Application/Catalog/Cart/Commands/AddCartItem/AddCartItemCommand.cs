using MediatR;

namespace Snagged.Application.Catalog.Cart.Commands.AddCartItem
{
    public class AddCartItemCommand : IRequest<int>
    {
        
        public int ItemId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}