using Mintcode.TuoTuo.v2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ResSendAuthMail : ResponseBaseModel<SendAuthMailResult>
    {

    }

    public class SendAuthMailResult
    {
        public string mail { set; get; }
        public string reSendMailToken { set; get; }
    }


}
