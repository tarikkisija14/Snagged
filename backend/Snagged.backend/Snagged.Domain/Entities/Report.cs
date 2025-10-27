using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public User Reporter { get; set; }
        public int? ReportedItemId { get; set; }
        public Item ReportedItem { get; set; }
        public int? ReportedUserId { get; set; }
        public User ReportedUser { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; }
        
        
        
    }
}
