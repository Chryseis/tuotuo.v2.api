using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models
{
    public class ReqChangeMemberRole : RequestBaseModel
    {
        public string mail { set; get; }

        public string roleCode { set; get; }
    }
}
