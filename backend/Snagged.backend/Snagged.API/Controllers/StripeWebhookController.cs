using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Abstractions;
using Stripe;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IAppDbContext _ctx;
        private readonly IConfiguration _config;

        public StripeWebhookController(IAppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            var webhookSecret = _config["Stripe:WebhookSecret"];
            Event stripeEvent;

            try
            {
                var signatureHeader = Request.Headers["Stripe-Signature"];
                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, webhookSecret);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    await HandleSuccess(stripeEvent);
                    break;

                case "payment_intent.payment_failed":
                    // Ako želiš, dodati logiku
                    break;
            }

            return Ok();
        }

        private async Task HandleSuccess(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            var order = _ctx.Orders.FirstOrDefault(o => o.StripePaymentIntentId == paymentIntent.Id);

            if (order == null && paymentIntent.Metadata != null)
            {
                if (paymentIntent.Metadata.TryGetValue("orderId", out string orderIdStr) &&
                    int.TryParse(orderIdStr, out int orderId))
                {
                    order = _ctx.Orders.FirstOrDefault(o => o.Id == orderId);

                    // Ako nađeš order po metadata, spremi StripePaymentIntentId za budućnost
                    if (order != null)
                    {
                        order.StripePaymentIntentId = paymentIntent.Id;
                    }
                }
            }




            if (order == null)
                return;

            var paidAmount = (decimal)paymentIntent.AmountReceived / 100m;

            var payment = new Snagged.Domain.Entities.Payment
            {
                StripePaymentIntentId = paymentIntent.Id,
                StripeChargeId = paymentIntent.LatestChargeId,
                PaidAmount = paidAmount,
                PaymentMethod = paymentIntent.PaymentMethodTypes?.FirstOrDefault(),
                PaymentDate = DateTime.UtcNow
            };

            _ctx.Payments.Add(payment);
            await _ctx.SaveChangesAsync();

            order.PaymentId = payment.Id;
            order.Status = "Paid";
            await _ctx.SaveChangesAsync();
        }
    }
}
