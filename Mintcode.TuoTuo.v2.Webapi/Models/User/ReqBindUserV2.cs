using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ReqBindUserV2 : RequestBaseModel
    {
        public string mail { get; set; }

        public string password { set; get; }

        public string redisId { get; set; }
   
    }
}
