using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Common
{
    public class FrontInputValidateErrorException: BaseException
    {
        public FrontInputValidateErrorException(string message):base(message)
        {

        }
    }
}
