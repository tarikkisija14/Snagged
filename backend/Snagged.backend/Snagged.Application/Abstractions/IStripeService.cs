using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Snagged.Application.Abstractions
{
    public interface IStripeService
    {
        Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency = "usd", CancellationToken ct = default, Dictionary<string, string> metadata = null);
        Task<PaymentIntent> GetPaymentIntentAsync(string intentId, CancellationToken ct = default);
    }
}
