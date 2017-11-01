using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.TuoTuo.v2.Infrastructure;


namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ReqSendAuthMail
    {
        /// <summary>
        /// 邮箱验证码类型
        /// </summary>
        public int codeType { set; get; }

        /// <summary>
        /// 验证码identity
        /// </summary>
        public string identity { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string mail { get; set; }

        /// <summary>
        /// 重发邮件的Token
        /// </summary>
        public string reSendEmailToken { set; get; }

    }
}
