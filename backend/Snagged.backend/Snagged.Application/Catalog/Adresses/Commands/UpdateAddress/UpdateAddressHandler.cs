using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Commands.UpdateAddress
{
    public class UpdateAddressHandler(IAppDbContext ctx): IRequestHandler<UpdateAddressCommand, bool>
    {
        public async Task<bool> Handle(UpdateAddressCommand req, CancellationToken ct)
        {
            var adr = await ctx.Addresses
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

            if (adr == null)
                return false;

            adr.UserId = req.UserId;
            adr.CityId = req.CityId;
            adr.Street = req.Street;
            adr.Lat = req.Lat;
            adr.Lng = req.Lng;

            await ctx.SaveChangesAsync();
            return true;
        }
    }
}
