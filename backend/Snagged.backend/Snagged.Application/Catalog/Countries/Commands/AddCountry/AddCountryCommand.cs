using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Commands.AddCountry
{
    public class AddCountryCommand : IRequest<int>
    {
        public string Name { get; set; }
    }
}
