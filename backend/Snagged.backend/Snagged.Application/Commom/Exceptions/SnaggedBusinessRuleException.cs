using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Commom.Exceptions
{
    public class SnaggedBusinessRuleException:Exception
    {
        public string Code { get; }

        public SnaggedBusinessRuleException(string code, string message)
            : base(message)
        {
            Code = code;
        }

        public SnaggedBusinessRuleException(string code, string message, Exception? innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}
