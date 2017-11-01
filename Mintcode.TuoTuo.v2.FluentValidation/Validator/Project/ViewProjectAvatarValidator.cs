using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class ViewProjectAvatarValidator : AbstractValidator<ReqViewProjectAvatar>
    {
        public ViewProjectAvatarValidator()
        {
            RuleFor(viewProjectAvatar => viewProjectAvatar.avatar).NotNull().NotEmpty().WithMessage("封面地址不能为空");          
        }
    }
}
