using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class BindUserV2Validator : AbstractValidator<ReqBindUserV2>
    {
        public BindUserV2Validator()
        {
            RuleFor(bindUserV2 => bindUserV2.mail).NotNull().WithMessage("邮箱不能为空").EmailAddress().WithMessage("邮箱格式不正确");

            RuleFor(bindUserV2 => bindUserV2.password).NotNull().WithMessage("密码不能为空");

        }
    }
}
