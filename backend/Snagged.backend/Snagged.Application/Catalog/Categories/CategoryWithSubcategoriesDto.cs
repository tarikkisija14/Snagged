using Snagged.Application.Catalog.Subcategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Categories
{
    public class CategoryWithSubcategoriesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubcategoryDto> Subcategories { get; set; } = new();
    }
}
