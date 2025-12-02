using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Cities.Queries.GetAllCities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Queries.GetCities
{
    public class GetCitiesHandler(IAppDbContext ctx): IRequestHandler<GetCitiesQuery, IEnumerable<CityDto>>
    {
        public async Task<IEnumerable<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var query = ctx.Cities.AsQueryable();

            if (request.CountryId.HasValue)
                query = query.Where(x => x.CountryId == request.CountryId);

            return await query
                .Select(x => new CityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CountryId = x.CountryId,
                    CountryName = x.Country.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
