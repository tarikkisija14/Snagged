using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Commands.DeleteAddress
{
    public class DeleteAddressHandler(IAppDbContext ctx) : IRequestHandler<DeleteAddressCommand, bool>
    {
        public async Task<bool> Handle(DeleteAddressCommand req, CancellationToken ct)
        {
            var adr = await ctx.Addresses.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

            if (adr == null)
                return false;

            ctx.Addresses.Remove(adr);
            await ctx.SaveChangesAsync();

            return true;
        }
    }
}
