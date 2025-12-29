using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Snagged.Application.Catalog.Payment.Commands.CreateStripePayment
{
    public class CreateStripePaymentHandler(IAppDbContext ctx, IStripeService stripeService) : IRequestHandler<CreateStripePaymentCommand, string>
    {
        public async Task<string> Handle(CreateStripePaymentCommand request, CancellationToken ct)
        {
            var order = await ctx.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);

            if (order == null)
                throw new KeyNotFoundException($"Order {request.OrderId} not found.");

            var amount = order.OrderItems.Sum(x => x.Price * x.Quantity);
            var metadata = new Dictionary<string, string>
            {
              { "orderId", request.OrderId.ToString() }
             };


            var intent = await stripeService.CreatePaymentIntentAsync(
               amount,
              request.Currency,
               ct, 
               metadata);

            
            order.StripePaymentIntentId = intent.Id;
            await ctx.SaveChangesAsync(ct);

            return intent.ClientSecret; 
        }
    }
}
