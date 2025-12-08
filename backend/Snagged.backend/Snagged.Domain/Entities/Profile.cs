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
        public User? User { get; set; }

        public string Username { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string Bio { get; set; } = string.Empty;
        public decimal AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
    }
}
