using Mintcode.TuoTuo.v2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ResVerifyEmailAuthCode : ResponseBaseModel<VerifyEmailAuthCodeResult>
    {

    }

    public class VerifyEmailAuthCodeResult
    {
        public string mail { set; get; }
        public string submitToken { set; get; }
    }
}
