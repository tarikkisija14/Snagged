using MediatR;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Commands.AddCity
{
    public class AddCityHandler(IAppDbContext ctx) : IRequestHandler<AddCityCommand, int>
    {
        public async Task<int> Handle(AddCityCommand request, CancellationToken cancellationToken)
        {
            var city = new City
            {
                Name = request.Name,
                CountryId = request.CountryId
            };

            ctx.Cities.Add(city);
            await ctx.SaveChangesAsync(cancellationToken);

            return city.Id;
        }
    }
}
