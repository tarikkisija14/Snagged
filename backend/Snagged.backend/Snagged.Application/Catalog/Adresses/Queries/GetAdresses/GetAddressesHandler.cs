using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Queries.GetAdresses
{
    public class GetAddressesHandler(IAppDbContext ctx): IRequestHandler<GetAddressesQuery, List<AddressDto>>
    {
        public async Task<List<AddressDto>> Handle(GetAddressesQuery request, CancellationToken ct)
        {
            var q = ctx.Addresses.AsQueryable();

            if (request.UserId.HasValue)
                q = q.Where(a => a.UserId == request.UserId.Value);

            return await q.Select(a => new AddressDto
            {
                Id = a.Id,
                UserId = a.UserId,
                CityId = a.CityId,
                CityName = a.City.Name,
                Street = a.Street,
                Lat = a.Lat,
                Lng = a.Lng
            }).ToListAsync(ct);
        }
    }
}
