using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Conversation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? ItemId { get; set; }
        public Item Item { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = string.Empty;
        
        
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
