using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Common
{
    public class UserInputValidateErrorException : BaseException
    {
        public UserInputValidateErrorException(string message):base(message)
        {

        }
    }
}
