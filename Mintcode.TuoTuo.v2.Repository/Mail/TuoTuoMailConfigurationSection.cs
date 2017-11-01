using Mintcode.TuoTuo.v2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Repository
{
    public class TuoTuoMailConfigurationSection : MailConfigurationSection
    {
        /// <summary>
        /// 邀请加入团队通知邮件正文
        /// </summary>
        [ConfigurationProperty("inviteContent", IsRequired = true)]
        public MailContentElement InviteContent { get { return this["inviteContent"] as MailContentElement; } }


        /// <summary>
        /// 发送验证码邮件正文
        /// </summary>
        [ConfigurationProperty("codeContent", IsRequired = true)]
        public MailContentElement CodeContent { get { return this["codeContent"] as MailContentElement; } }
    }
}
