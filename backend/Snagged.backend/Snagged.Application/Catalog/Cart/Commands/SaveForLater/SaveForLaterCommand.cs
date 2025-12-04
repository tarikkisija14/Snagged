using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Cart.Commands.SaveForLater
{
    public class SaveForLaterCommand : IRequest<Unit>
    {
        public int CartId { get; set; }
    }
}
