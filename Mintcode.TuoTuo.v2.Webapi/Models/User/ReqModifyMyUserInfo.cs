using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.User
{
    public class ReqModifyMyUserInfo:RequestBaseModel
    {
        public string userName { set; get; }

        public string avatarToken { set; get; }

        public string mobile { set; get; }
    }
}
