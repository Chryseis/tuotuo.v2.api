using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class BindUserValidator : AbstractValidator<ReqBindUser>
    {
        public BindUserValidator()
        {
            RuleFor(bindUser => bindUser.mail).NotNull().WithMessage("邮箱不能为空").EmailAddress().WithMessage("邮箱格式不正确");
            
        }
    }
}
