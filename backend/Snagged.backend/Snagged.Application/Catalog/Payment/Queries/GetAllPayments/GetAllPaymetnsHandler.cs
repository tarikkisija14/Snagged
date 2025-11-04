using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Snagged.Application.Catalog.Payment.Queries.GetAllPayments
{
    public class GetAllPaymentsHandler(IAppDbContext ctx) : IRequestHandler<GetAllPaymentsQuery, List<PaymentDto>>
    {
        public async Task<List<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken ct)
        {
            return await ctx.Payments
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    PaymentMethod = p.PaymentMethod,
                    PaidAmount = p.PaidAmount,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync(ct);
        }
    }
}
