using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class CreateSprintValidator : AbstractValidator<ReqCreateSprint>
    {
        public CreateSprintValidator()
        {
            RuleFor(createSprint => createSprint.releaseID)
                .Must(s => s > 0).WithMessage("Release ID不能为空");
            RuleFor(createSprint => createSprint.startTime)
                .Must(s => s > 0).WithMessage("开始时间不能为空");
            RuleFor(createSprint => createSprint.endTime)
                .Must(s => s > 0).WithMessage("结束时间不能为空");
            RuleFor(editSprint => editSprint.startTime)
                .Must((editSprint, startTime) => startTime <= editSprint.endTime).WithMessage("开始时间不能大于结束时间");
        }
    }
}
