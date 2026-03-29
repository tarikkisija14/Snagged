using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Payment.Queries.GetAllPaymentsByUser
{
    public class GetAllPaymentsByUserHandler(IAppDbContext ctx)
        : IRequestHandler<GetPaymentsByUserQuery, List<PaymentDto>>
    {
        public async Task<List<PaymentDto>> Handle(GetPaymentsByUserQuery request, CancellationToken ct)
        {
            return await ctx.Payments
                .AsNoTracking()
                .Where(p => p.Order.BuyerId == request.UserId)
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