using FluentValidation;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class ChangeMemberRoleValidator : AbstractValidator<ReqChangeMemberRole>
    {
        public ChangeMemberRoleValidator()
        {
            RuleFor(changeMemberRole => changeMemberRole.mail).NotNull().NotEmpty().WithMessage("修改用户邮箱不能为空").EmailAddress().WithMessage("修改用户邮箱格式不正确");
            RuleFor(changeMemberRole => changeMemberRole.roleCode).NotNull().NotEmpty().WithMessage("角色码不能为空")
                .Must(roleCode=>RoleCode.CheckCode(roleCode)).WithMessage("不存在当前角色码");
        }
    }
}
