using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Queries.GetCountriesById
{
   public class GetCountriesByIdHandler(IAppDbContext ctx): IRequestHandler<GetCountriesByIdQuery, CountryDto?>
    {
        public async Task<CountryDto?> Handle(GetCountriesByIdQuery request, CancellationToken ct)
        {
            return await ctx.Countries
                .Where(c => c.Id == request.Id)
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
