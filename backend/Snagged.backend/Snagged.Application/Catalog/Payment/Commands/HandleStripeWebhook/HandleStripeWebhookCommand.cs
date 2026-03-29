using MediatR;

namespace Snagged.Application.Catalog.Payment.Commands.HandleStripeWebhook
{
    public class HandleStripeWebhookCommand : IRequest
    {
        public required string StripePaymentIntentId { get; init; }
        public required string? StripeChargeId { get; init; }
        public required decimal PaidAmount { get; init; }
        public required string? PaymentMethod { get; init; }
        public required int? OrderIdHint { get; init; }
    }
}