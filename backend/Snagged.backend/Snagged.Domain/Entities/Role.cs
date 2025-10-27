using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Role
    {
        public int Id {  get; set; }
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
