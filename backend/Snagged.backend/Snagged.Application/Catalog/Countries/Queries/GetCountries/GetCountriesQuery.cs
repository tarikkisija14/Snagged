using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Queries.GetCountries
{
    public class GetCountriesQuery : IRequest<List<CountryDto>>
    {
    }
}
