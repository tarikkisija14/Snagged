using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Items.Commands.DeleteItem
{
    public class DeleteItemCommand :IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
