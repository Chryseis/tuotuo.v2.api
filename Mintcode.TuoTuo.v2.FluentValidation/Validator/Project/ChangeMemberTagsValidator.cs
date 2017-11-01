using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class ChangeMemberTagsValidator : AbstractValidator<ReqChangeMemberTags>
    {
        public ChangeMemberTagsValidator()
        {
            RuleFor(changeMemberTags => changeMemberTags.mail).NotNull().NotEmpty().WithMessage("修改用户邮箱不能为空").EmailAddress().WithMessage("修改用户邮箱格式不正确");
        }
    }
}
