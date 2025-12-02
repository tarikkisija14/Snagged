using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Queries.GetAllCities
{
    public class GetCitiesQuery:IRequest<IEnumerable<CityDto>>
    {
        public int? CountryId { get; set; }
    }
}
