using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.SaveForLater
{
    public class SaveForLaterHandler(IAppDbContext ctx) : IRequestHandler<SaveForLaterCommand, Unit>
    {
        public async Task<Unit> Handle(SaveForLaterCommand request, CancellationToken ct)
        {
            var cart = await ctx.Carts
             .Include(c => c.CartItems)
             .FirstOrDefaultAsync(c => c.Id == request.CartId, ct);

            if (cart == null)
                throw new KeyNotFoundException($"Cart {request.CartId} not found.");

            cart.IsSavedForLater = true; 
            await ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
