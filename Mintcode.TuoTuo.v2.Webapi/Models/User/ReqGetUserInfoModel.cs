using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ReqGetUserInfoModel : RequestBaseModel
    {
        public string mail { set; get; }
    }
}
