using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.User
{
    public class ModifyMyUserInfoValidator : AbstractValidator<ReqModifyMyUserInfo>
    {
        public ModifyMyUserInfoValidator()
        {
            RuleFor(modifyMyUserInfo => modifyMyUserInfo.userName).NotNull().NotEmpty().WithMessage("用户名不能为空");
        }
    }
}
