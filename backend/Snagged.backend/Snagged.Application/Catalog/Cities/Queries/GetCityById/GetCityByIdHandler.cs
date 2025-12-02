using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Queries.GetCityById
{
    public class GetCityByIdHandler(IAppDbContext ctx) : IRequestHandler<GetCityByIdQuery, CityDto?>
    {
        public async Task<CityDto?> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            return await ctx.Cities
           .Where(x => x.Id == request.Id)
           .Select(x => new CityDto
           {
               Id = x.Id,
               Name = x.Name,
               CountryId = x.CountryId,
               CountryName = x.Country.Name
           })
           .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
