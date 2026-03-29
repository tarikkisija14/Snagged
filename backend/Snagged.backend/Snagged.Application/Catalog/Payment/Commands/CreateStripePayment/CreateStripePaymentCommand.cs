using MediatR;

namespace Snagged.Application.Catalog.Payment.Commands.CreateStripePayment
{
    public class CreateStripePaymentCommand : IRequest<string>
    {
        public int OrderId { get; set; }

       
        public string Currency { get; set; } = "usd";
    }
}