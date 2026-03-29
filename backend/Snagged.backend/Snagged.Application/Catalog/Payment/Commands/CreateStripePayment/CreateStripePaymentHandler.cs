using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Payment.Commands.CreateStripePayment
{
    public class CreateStripePaymentHandler(IAppDbContext ctx, IStripeService stripeService)
        : IRequestHandler<CreateStripePaymentCommand, string>
    {
        public async Task<string> Handle(CreateStripePaymentCommand request, CancellationToken ct)
        {
            
            var order = await ctx.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);

            if (order is null)
                throw new SnaggedNotFoundException($"Order with id {request.OrderId} was not found.");

            var amount = order.OrderItems.Sum(x => x.Price * x.Quantity);
            var metadata = new Dictionary<string, string> { { "orderId", request.OrderId.ToString() } };

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