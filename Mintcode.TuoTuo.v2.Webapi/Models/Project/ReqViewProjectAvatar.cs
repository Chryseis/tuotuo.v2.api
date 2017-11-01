using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models
{
    public class ReqViewProjectAvatar:RequestBaseModel
    {
        public string avatar { set; get; }

        public int width { set; get; }

        public int height { set; get; }
    }
}
