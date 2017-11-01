using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class RemoveMemberValidator : AbstractValidator<ReqRemoveMember>
    {
        public RemoveMemberValidator()
        {
            RuleFor(remove => remove.mail).NotNull().NotEmpty().WithMessage("移除用户邮箱不能为空").EmailAddress().WithMessage("移除用户邮箱格式不正确");
        }
    }
}
