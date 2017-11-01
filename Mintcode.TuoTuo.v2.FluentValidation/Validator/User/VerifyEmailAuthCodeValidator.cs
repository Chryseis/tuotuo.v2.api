using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
   public class VerifyEmailAuthCodeValidator : AbstractValidator<ReqVerifyEmailAuthCode>
    {
        public VerifyEmailAuthCodeValidator()
        {
            RuleFor(verifyEmailAuthCode => verifyEmailAuthCode.mail).NotNull().WithMessage("邮箱不能为空").EmailAddress().WithMessage("邮箱格式不正确");
            RuleFor(verifyEmailAuthCode => verifyEmailAuthCode.code).NotNull().WithMessage("验证码不能为空");
        }
    }
}
