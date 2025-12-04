using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Payment
{
    public class PaymentStatus
    {
        public const string Pending = "pending";
        public const string Processing = "processing";
        public const string Paid = "paid";
        public const string Failed = "failed";
        public const string Refunded = "refunded";

    }
}
