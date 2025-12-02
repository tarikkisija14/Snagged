using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Commands.DeleteCity
{
    public class DeleteCityHandler(IAppDbContext ctx) : IRequestHandler<DeleteCityCommand, bool>
    {
        public async Task<bool> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var entity = await ctx.Cities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
                return false;

            
            if (await ctx.Addresses.AnyAsync(a => a.CityId == request.Id, cancellationToken))
                throw new InvalidOperationException("Cannot delete city because it has addresses.");

            ctx.Cities.Remove(entity);
            await ctx.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
