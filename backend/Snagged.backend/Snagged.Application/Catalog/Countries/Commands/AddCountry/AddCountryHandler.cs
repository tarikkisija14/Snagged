using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Commands.AddCountry
{
    internal class AddCountryHandler(IAppDbContext ctx): IRequestHandler<AddCountryCommand, int>
    {
        public async Task<int> Handle(AddCountryCommand request, CancellationToken ct)
        {
            var country = new Country
            {
                Name = request.Name
            };

            ctx.Countries.Add(country);
            await ctx.SaveChangesAsync(ct);

            return country.Id;
        }
    }
}
