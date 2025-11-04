using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.ClearCart
{
    public class ClearCartHandler(IAppDbContext ctx) : IRequestHandler<ClearCartCommand,Unit>
    {
        public async Task<Unit> Handle(ClearCartCommand request, CancellationToken ct)
        {
            var cart = await ctx.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, ct);

            if (cart != null)
            {
                ctx.CartItems.RemoveRange(cart.CartItems);
                await ctx.SaveChangesAsync(ct);
            }

            return Unit.Value;
        }
    }
}
