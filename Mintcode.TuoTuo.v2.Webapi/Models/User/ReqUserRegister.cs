using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.TuoTuo.v2.Infrastructure;



namespace Mintcode.TuoTuo.v2.Webapi
{

    public class ReqUserRegister:RequestBaseModel
    {
        public string mail { get; set; }

        public string password { get; set; }

        public string name { get; set; }

        public string redisId { get; set; }

        public string submitToken { set; get; }
    }
}
