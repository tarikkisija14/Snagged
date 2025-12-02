using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cities.Queries.GetCityById
{
    public class GetCityByIdQuery : IRequest<CityDto?>
    {
        public int Id { get; set; }
        public GetCityByIdQuery(int id) { Id = id; }
    }
}
