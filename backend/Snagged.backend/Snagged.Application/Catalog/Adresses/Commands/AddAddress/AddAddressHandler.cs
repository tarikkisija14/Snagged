using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Commands.AddAddress
{
    public class AddAddressHandler(IAppDbContext ctx): IRequestHandler<AddAddressCommand, int>
    {
        public async Task<int> Handle(AddAddressCommand req, CancellationToken ct)
        {
            var adr = new Address
            {
                UserId = req.UserId,
                CityId = req.CityId,
                Street = req.Street,
                Lat = req.Lat,
                Lng = req.Lng
            };

            ctx.Addresses.Add(adr);
            await ctx.SaveChangesAsync();

            return adr.Id;
        }
    }
}
