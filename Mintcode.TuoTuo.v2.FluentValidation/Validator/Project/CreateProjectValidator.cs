using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class CreateProjectValidator : AbstractValidator<ReqCreateProject>
    {
        public CreateProjectValidator()
        {
            RuleFor(register => register.projectName).NotNull().NotEmpty().WithMessage("项目名称不能为空");
        }
    }
}
