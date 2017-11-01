using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.TuoTuo.v2.Infrastructure;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ResUserModel:ResponseBaseModel<User>
    {

    }

    public class User
    {
        public int userId { get; set; }

        public string userName { get; set; }

        public int sex { get; set; }

        public string address { get; set; }
    }
}
