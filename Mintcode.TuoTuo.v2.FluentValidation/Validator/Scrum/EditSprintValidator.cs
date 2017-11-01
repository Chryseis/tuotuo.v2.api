using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class EditSprintValidator : AbstractValidator<ReqEditSprint>
    {
        public EditSprintValidator()
        {
            RuleFor(editSprint => editSprint.sprintID)
            .Must(s => s > 0).WithMessage("Sprint ID不能为空");
            RuleFor(editSprint => editSprint.startTime)
                .NotNull().NotEmpty().WithMessage("开始时间不能为空")
                .Must(ValidateHelper.BeAValidDate).WithMessage("开始时间格式不正确");
            RuleFor(editSprint => editSprint.endTime)
                .NotNull().NotEmpty().WithMessage("结束时间不能为空")
                .Must(ValidateHelper.BeAValidDate).WithMessage("结束时间格式不正确");
            RuleFor(editSprint => editSprint.startTime)
                .Must((editSprint, startTime) => DateTime.Parse(startTime) < DateTime.Parse(editSprint.endTime)).WithMessage("开始时间不能大于结束时间");
        }
    }
}
