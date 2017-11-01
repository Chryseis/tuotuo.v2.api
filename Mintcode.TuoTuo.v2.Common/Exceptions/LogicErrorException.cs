using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Common
{
    public class LogicErrorException: BaseException
    {
        public LogicErrorException(string message):base(message)
        {
                
        }
    }
}
