using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Subcategories.Commands.DeleteSubCategory
{
    public class DeleteSubcategoryCommand:IRequest<bool>
    {
        public int Id {  get; set; }

    }
}
