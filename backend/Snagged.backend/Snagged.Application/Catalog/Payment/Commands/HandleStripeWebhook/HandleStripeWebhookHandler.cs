using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Payment;

namespace Snagged.Application.Catalog.Payment.Commands.HandleStripeWebhook
{
    public class HandleStripeWebhookHandler(IAppDbContext ctx, IWebPushService pushService)
        : IRequestHandler<HandleStripeWebhookCommand>
    {
        public async Task Handle(HandleStripeWebhookCommand request, CancellationToken ct)
        {
            var alreadyProcessed = await ctx.Payments
                .AnyAsync(p => p.StripePaymentIntentId == request.StripePaymentIntentId, ct);
            if (alreadyProcessed) return;

            var order = await ResolveOrderAsync(request, ct);
            if (order is null) return;

            var payment = new Domain.Entities.Payment
            {
                StripePaymentIntentId = request.StripePaymentIntentId,
                StripeChargeId = request.StripeChargeId,
                PaidAmount = request.PaidAmount,
                PaymentMethod = request.PaymentMethod ?? string.Empty,
                PaymentDate = DateTime.UtcNow,
                OrderId = order.Id
            };

            ctx.Payments.Add(payment);

           
            order.PaymentId = payment.Id;
            order.Status = PaymentStatus.Paid;

            var orderItems = await ctx.OrderItems
                .Include(oi => oi.Item)
                .Where(oi => oi.OrderId == order.Id)
                .ToListAsync(ct);

            
            ctx.Notifications.Add(new Domain.Entities.Notification
            {
                UserId = order.BuyerId,
                Message = $"Payment for order #{order.Id} was successful. Thank you for your purchase!",
                NotificationType = "Payment",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

           
            foreach (var oi in orderItems)
            {
                if (oi.Item is null) continue;

                oi.Item.IsSold = true;

                ctx.Notifications.Add(new Domain.Entities.Notification
                {
                    UserId = oi.Item.UserId,
                    Message = $"Your item \"{oi.Item.Title}\" has been sold!",
                    NotificationType = "Sale",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });
            }

           
            await ctx.SaveChangesAsync(ct);

           
            await pushService.SendAsync(
                order.BuyerId,
                "Payment confirmed ",
                $"Your payment for order #{order.Id} was successful. Thank you!",
                ct);

            foreach (var oi in orderItems.Where(oi => oi.Item is not null))
            {
                await pushService.SendAsync(
                    oi.Item!.UserId,
                    "Item sold & payment confirmed ",
                    $"\"{oi.Item.Title}\" has been sold and payment received!",
                    ct);
            }
        }

        private async Task<Domain.Entities.Order?> ResolveOrderAsync(
            HandleStripeWebhookCommand request, CancellationToken ct)
        {
            var order = await ctx.Orders
                .FirstOrDefaultAsync(o => o.StripePaymentIntentId == request.StripePaymentIntentId, ct);

            if (order is not null) return order;
            if (!request.OrderIdHint.HasValue) return null;

            order = await ctx.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderIdHint.Value, ct);
            if (order is not null)
                order.StripePaymentIntentId = request.StripePaymentIntentId;

            return order;
        }
    }
}