using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Identity
{
    public class AccessTokenModel
    {

        public string access_token { set; get; }

        public string token_type { set; get; }

        public long expires_in { set; get; }

        public string refresh_token { set; get; }
    }
}
