using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Commands.AddCity
{
    public class AddCityCommand : IRequest<int>
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
