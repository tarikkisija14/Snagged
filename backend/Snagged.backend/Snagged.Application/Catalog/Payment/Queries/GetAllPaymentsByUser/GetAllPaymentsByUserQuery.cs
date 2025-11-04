using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Payment.Queries.GetAllPaymentsByUser
{
    public class GetPaymentsByUserQuery : IRequest<List<PaymentDto>>
    {
        public int UserId { get; set; }
    }
}
