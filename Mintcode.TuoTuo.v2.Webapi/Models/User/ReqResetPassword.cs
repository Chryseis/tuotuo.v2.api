using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.TuoTuo.v2.Infrastructure;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ReqResetPassword : RequestBaseModel
    {
        public string mail { get; set; }

        public string password { get; set; }

        public string submitToken { set; get; }
    }
}
