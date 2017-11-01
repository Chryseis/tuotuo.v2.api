using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Identity
{
    public class UnBindErrorOAuthModel : ErrorOAtuhModel
    {
        public string relationAccountID { set; get; }
    }
}
