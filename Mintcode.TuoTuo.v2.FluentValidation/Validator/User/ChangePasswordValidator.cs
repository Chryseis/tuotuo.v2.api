using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class ChangePasswordValidator : AbstractValidator<ReqChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(changePassword => changePassword.mail).NotNull().WithMessage("邮箱不能为空").EmailAddress().WithMessage("邮箱格式不正确");
            RuleFor(changePassword => changePassword.oldPassword).NotNull().WithMessage("原密码不能为空");
            RuleFor(changePassword => changePassword.newPassword).NotNull().WithMessage("新密码不能为空");
        }
    }
}
