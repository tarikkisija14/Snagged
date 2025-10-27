using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
