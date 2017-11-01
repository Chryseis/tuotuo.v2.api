using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class ModifyProjectValidator : AbstractValidator<ReqModifyProject>
    {
        public ModifyProjectValidator()
        {
            RuleFor(modify => modify.projectName).NotNull().NotEmpty().WithMessage("项目名称不能为空");
        }
    }
}
