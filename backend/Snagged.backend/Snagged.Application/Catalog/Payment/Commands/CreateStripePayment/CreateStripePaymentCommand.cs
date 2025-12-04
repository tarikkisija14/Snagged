using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Payment.Commands.CreateStripePayment
{
    public class CreateStripePaymentCommand : IRequest<string>
    {
        public int OrderId { get; set; }
        public string Currency { get; set; } = "usd";
    }
}
