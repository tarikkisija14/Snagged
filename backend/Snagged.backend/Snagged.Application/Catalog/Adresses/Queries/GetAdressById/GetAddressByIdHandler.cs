using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Queries.GetAdressesById
{
    public class GetAddressByIdHandler(IAppDbContext ctx): IRequestHandler<GetAddressByIdQuery, AddressDto?>
    {
        public async Task<AddressDto?> Handle(GetAddressByIdQuery request, CancellationToken ct)
        {
            return await ctx.Addresses
            .Where(a => a.Id == request.Id)
            .Select(a => new AddressDto
            {
                Id = a.Id,
                UserId = a.UserId,
                CityId = a.CityId,
                CityName = a.City.Name,
                Street = a.Street,
                Lat = a.Lat,
                Lng = a.Lng
            })
            .FirstOrDefaultAsync(ct);
        }
    }
}
