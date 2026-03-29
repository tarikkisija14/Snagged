using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Payment.Commands.HandleStripeWebhook;
using Stripe;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class StripeWebhookController(IMediator mediator, IConfiguration config) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var webhookSecret = config["Stripe:WebhookSecret"];

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    webhookSecret);
            }
            catch (StripeException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

            if (stripeEvent.Type == "payment_intent.succeeded")
                await HandlePaymentSucceededAsync(stripeEvent);

            return Ok();
        }

        private async Task HandlePaymentSucceededAsync(Event stripeEvent)
        {
            if (stripeEvent.Data.Object is not PaymentIntent paymentIntent)
                return;

            int? orderIdHint = null;
            if (paymentIntent.Metadata.TryGetValue("orderId", out var orderIdStr)
                && int.TryParse(orderIdStr, out var parsed))
            {
                orderIdHint = parsed;
            }

            var command = new HandleStripeWebhookCommand
            {
                StripePaymentIntentId = paymentIntent.Id,
                StripeChargeId = paymentIntent.LatestChargeId,
                PaidAmount = (decimal)paymentIntent.AmountReceived / 100m,
                PaymentMethod = paymentIntent.PaymentMethodTypes?.FirstOrDefault(),
                OrderIdHint = orderIdHint
            };

            await mediator.Send(command);
        }
    }
}