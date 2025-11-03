using MediatR;
using Snagged.Application.Items.Queries.GetItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Items.Queries.GetItemsById
{
    public class GetItemByIdQuery :IRequest<ItemDto>
    {
        public int Id { get; set; } 
        public GetItemByIdQuery(int id) 
        {
            Id = id;
        }
    }
}
