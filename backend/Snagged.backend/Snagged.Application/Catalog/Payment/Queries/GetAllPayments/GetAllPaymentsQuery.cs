using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Payment.Queries.GetAllPayments
{
    public class GetAllPaymentsQuery : IRequest<List<PaymentDto>>
    {

    }
}
