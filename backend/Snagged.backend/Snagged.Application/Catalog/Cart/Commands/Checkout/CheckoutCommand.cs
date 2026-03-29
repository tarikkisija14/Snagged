using MediatR;

namespace Snagged.Application.Catalog.Cart.Commands.Checkout
{
    public class CheckoutCommand : IRequest<int>
    {
        

        public int? AddressId { get; set; }
    }
}