using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Items.Commands.AddItem
{
    public class AddItemCommand :IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Condition { get; set; }
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public int UserId { get; set; } 
        public List<string> ImageUrls { get; set; } = new();
    }
}
