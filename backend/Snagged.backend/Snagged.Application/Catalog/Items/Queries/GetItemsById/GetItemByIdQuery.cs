using MediatR;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Application.Catalog.Items.Queries.GetItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Queries.GetItemsById
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
