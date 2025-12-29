using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string? StripePaymentIntentId { get; set; }
    }
}
