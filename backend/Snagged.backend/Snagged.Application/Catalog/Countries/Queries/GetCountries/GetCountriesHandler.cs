using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Queries.GetCountries
{
    public class GetCountriesHandler(IAppDbContext ctx): IRequestHandler<GetCountriesQuery, List<CountryDto>>
    {
        public async Task<List<CountryDto>> Handle(GetCountriesQuery request, CancellationToken ct)
        {
            return await ctx.Countries
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync(ct);
        }
    }
}
