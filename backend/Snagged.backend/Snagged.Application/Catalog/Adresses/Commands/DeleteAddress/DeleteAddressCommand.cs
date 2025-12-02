using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Adresses.Commands.DeleteAddress
{
    public class DeleteAddressCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
