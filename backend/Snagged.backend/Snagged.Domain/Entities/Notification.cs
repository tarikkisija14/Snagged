using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Message { get; set; } = string.Empty;
        public string NotificationType { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
