using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Queries.GetCountriesById
{
    public class GetCountriesByIdQuery:IRequest<CountryDto>
    {
        public int Id {  get; set; }

        public GetCountriesByIdQuery(int id)
        {
            Id = id;
        }
    }
}
