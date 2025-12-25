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
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int OrderId { get; set; } //popravljeno ovo, narudzba i payment trebaju biti 1:1
        public Order? Order { get; set; }
        public string StripePaymentIntentId { get; set; } = string.Empty;
        public string? StripeChargeId { get; set; }


    }
}
