using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Payment.Commands.CreatePayment
{
    public class CreatePaymentHandler(IAppDbContext ctx) : IRequestHandler<CreatePaymentCommand, int>
    {
        public async Task<int> Handle(CreatePaymentCommand request, CancellationToken ct)
        {
            
            var order = await ctx.Orders.FindAsync(new object[] { request.OrderId }, ct);
            if (order == null)
                throw new KeyNotFoundException($"Order {request.OrderId} not found.");

            
            var payment = new Snagged.Domain.Entities.Payment
            {
                PaymentMethod = request.PaymentMethod,
                PaidAmount = request.Amount,
                PaymentDate = DateTime.Now
            };

            ctx.Payments.Add(payment);

            
            order.Payment = payment;
            order.Status = "Paid";

            await ctx.SaveChangesAsync(ct);

            return payment.Id;
        }
    }
}
