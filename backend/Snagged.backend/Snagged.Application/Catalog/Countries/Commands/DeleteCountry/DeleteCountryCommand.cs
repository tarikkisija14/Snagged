using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
