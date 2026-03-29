using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Payment.Queries.GetAllPayments
{
    public class GetAllPaymentsHandler(IAppDbContext ctx) : IRequestHandler<GetAllPaymentsQuery, List<PaymentDto>>
    {
        public async Task<List<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken ct)
        {
            return await ctx.Payments
                .AsNoTracking()
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