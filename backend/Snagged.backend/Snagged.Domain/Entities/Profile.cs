using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
        public decimal AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
    }
}
