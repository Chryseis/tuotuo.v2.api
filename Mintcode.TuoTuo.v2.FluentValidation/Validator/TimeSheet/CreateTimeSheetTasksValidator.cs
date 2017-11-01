using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.TimeSheet
{
    public class CreateTimeSheetTasksValidator : AbstractValidator<ReqCreateTimeSheetTasks>
    {
        public CreateTimeSheetTasksValidator()
        {
            RuleFor(create => create.sheetID).Must(s => s > 0).WithMessage("TimeSheet ID不能为空");
        }
    }
}
