using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }

        public int ReviewerId { get; set; }
        public User? Reviewer { get; set; }

        public int ReviewedUserId { get; set; }
        public User? ReviewedUser { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    }
}
