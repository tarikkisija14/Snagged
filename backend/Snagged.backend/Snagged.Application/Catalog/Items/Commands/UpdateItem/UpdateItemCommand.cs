using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Commands.UpdateItem
{
    public class UpdateItemCommand: IRequest<Unit>
    {
        [JsonIgnore] 
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required string Condition { get; set; }
        public required bool IsSold { get; set; }
        public required int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
    }
}
