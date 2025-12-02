using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Commands.UpdateCity
{
    public class UpdateCityHandler(IAppDbContext ctx) : IRequestHandler<UpdateCityCommand, bool>
    {
        public async Task<bool> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var entity = await ctx.Cities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
                return false;

            entity.Name = request.Name;
            entity.CountryId = request.CountryId;

            await ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
