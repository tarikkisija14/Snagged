using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Queries.GetItemImages
{
    public class GetItemImagesQuery : IRequest<List<ItemImageDto>>
    {
        public int ItemId { get; set; }
    }
}
