using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Commands.UpdateCountry
{
    public class UpdateCountryHandler(IAppDbContext ctx): IRequestHandler<UpdateCountryCommand, bool>
    {
        public async Task<bool> Handle(UpdateCountryCommand request, CancellationToken ct)
        {
            var country = await ctx.Countries
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (country == null)
                return false;

            country.Name = request.Name;

            await ctx.SaveChangesAsync(ct);
            return true;
        }
    }
}
