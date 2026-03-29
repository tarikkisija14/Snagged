using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Payment.Commands.CreatePayment
{
    
    public class CreatePaymentHandler(IAppDbContext ctx) : IRequestHandler<CreatePaymentCommand, int>
    {
        public async Task<int> Handle(CreatePaymentCommand request, CancellationToken ct)
        {
            var order = await ctx.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);

            if (order is null)
                throw new SnaggedNotFoundException($"Order with id {request.OrderId} was not found.");

            
            if (order.PaymentId.HasValue)
                throw new SnaggedConflictException($"Order {request.OrderId} has already been paid.");

            var payment = new Domain.Entities.Payment
            {
                PaymentMethod = request.PaymentMethod,
                PaidAmount = request.Amount,
                PaymentDate = DateTime.UtcNow,
                OrderId = order.Id,
                StripePaymentIntentId = request.StripePaymentIntentId
            };

            ctx.Payments.Add(payment);
            order.Payment = payment;
            order.Status = PaymentStatus.Paid;

            foreach (var orderItem in order.OrderItems)
                orderItem.Item.IsSold = true;

            await ctx.SaveChangesAsync(ct);
            return payment.Id;
        }
    }
}