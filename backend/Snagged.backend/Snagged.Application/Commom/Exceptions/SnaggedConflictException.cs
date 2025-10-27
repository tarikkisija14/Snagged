using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Commom.Exceptions
{
    public class SnaggedConflictException:Exception
    {
        public SnaggedConflictException(string message) : base(message) { }
    }
}
