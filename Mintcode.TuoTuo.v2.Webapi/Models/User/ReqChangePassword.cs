using Mintcode.TuoTuo.v2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ReqChangePassword : RequestBaseModel
    {
        public string mail { get; set; }

        public string oldPassword { set; get; }

        public string newPassword { get; set; }
    }
}
