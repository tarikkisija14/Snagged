using MediatR;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Snagged.Application.Catalog.Payment.Queries.GetAllPaymentsByUser
{
    //public class GetPaymentsByUserHandler(IAppDbContext ctx) : IRequestHandler<GetPaymentsByUserQuery, List<PaymentDto>>
    //{
    //    //public async Task<List<PaymentDto>> Handle(GetPaymentsByUserQuery request, CancellationToken ct)
    //    //{
    //    //    //return await ctx.Payments
    //    //    //    .Where(p => p.Order.Any(o => o.BuyerId == request.UserId))
    //    //    //    .Select(p => new PaymentDto
    //    //    //    {
    //    //    //        Id = p.Id,
    //    //    //        PaymentMethod = p.PaymentMethod,
    //    //    //        PaidAmount = p.PaidAmount,
    //    //    //        PaymentDate = p.PaymentDate
    //    //    //    })
    //    //    //    .ToListAsync(ct);
    //    //}
    //}
}
