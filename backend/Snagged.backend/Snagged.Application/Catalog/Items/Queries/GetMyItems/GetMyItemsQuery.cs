using MediatR;
using Snagged.Application.Catalog.Items.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Queries.GetMyItems
{
    public class GetMyItemsQuery : IRequest<List<ItemDto>>
    {

    }
}
