using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.User
{
    public class ReqGetUserAvatar:RequestBaseModel
    {
        public string selectUserMail { set; get; }

        public int width { set; get; }

        public int height { set; get; }
    }
}
