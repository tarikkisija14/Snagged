using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Commands.DeleteCountry
{
    public class DeleteCountryHandler(IAppDbContext ctx): IRequestHandler<DeleteCountryCommand, bool>
    {
        public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken ct)
        {
            var country = await ctx.Countries
               .Include(c => c.Cities)
               .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (country == null)
                return false;

            if (country.Cities.Any())
                throw new InvalidOperationException("Cannot delete country that contains cities.");

            ctx.Countries.Remove(country);
            await ctx.SaveChangesAsync(ct);
            return true;
        }
    }
}
