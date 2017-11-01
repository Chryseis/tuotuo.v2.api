using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class SendAuthMailValidator : AbstractValidator<ReqSendAuthMail>
    {
        public SendAuthMailValidator()
        {
            RuleFor(sendAuthMail => sendAuthMail.mail).NotNull().WithMessage("邮箱不能为空").EmailAddress().WithMessage("邮箱格式不正确");
            RuleFor(sendAuthMail => sendAuthMail.code).NotNull().When(sendAuthMail => string.IsNullOrWhiteSpace(sendAuthMail.reSendEmailToken)).WithMessage("验证码不能为空");

        }
    }
}
