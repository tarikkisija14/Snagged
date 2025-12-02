using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Categories.Commands.AddCategory
{
    public class AddCategoryCommand:IRequest<int>
    {
        public string Name {  get; set; }
    }
}
