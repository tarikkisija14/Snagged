using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Commands.DeleteCity
{
    public class DeleteCityCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
