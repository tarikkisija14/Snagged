using Microsoft.Extensions.Options;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Snagged.Application.Commom.Helper
{
    public class StripeService:IStripeService
    {
        private readonly StripeSettings _settings;
        private readonly PaymentIntentService _paymentIntentService;

        public StripeService(IOptions<StripeSettings> cfg)
        {
            _settings = cfg.Value;
            _paymentIntentService = new PaymentIntentService();
        }

        public Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency = "usd", CancellationToken ct = default, Dictionary<string, string> metadata = null)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100m),
                Currency = currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions { Enabled = true },
                Metadata = metadata ?? new Dictionary<string, string>()
            };

            return _paymentIntentService.CreateAsync(options, cancellationToken: ct);
        }

        public async Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId, CancellationToken cancellationToken = default)
        {
            return await _paymentIntentService.GetAsync(paymentIntentId, cancellationToken: cancellationToken);
        }


    }
}
