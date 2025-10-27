using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Commom.Exceptions
{
    public class SnaggedNotFoundException:Exception
    {
        public SnaggedNotFoundException(string message) : base(message) { }
    }
}
