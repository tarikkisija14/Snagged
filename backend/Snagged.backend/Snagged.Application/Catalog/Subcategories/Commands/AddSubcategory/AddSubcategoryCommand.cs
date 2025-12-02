using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Commands.AddSubcategory
{
    public class AddSubcategoryCommand:IRequest<int>
    {
        public string Name {  get; set; }
        public int CategoryId {  get; set; }
    }
}
