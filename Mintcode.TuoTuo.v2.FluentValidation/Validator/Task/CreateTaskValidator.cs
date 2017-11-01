using FluentValidation;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Webapi.Models;
using Mintcode.TuoTuo.v2.Webapi.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.Task
{
    public class CreateTaskValidator : AbstractValidator<ReqCreateTask>
    {
        public CreateTaskValidator()
        {
            RuleFor(createTask => createTask.title).NotNull().NotEmpty().WithMessage("Backlog标题不能为空");
            RuleFor(createTask => createTask.backLogID).Must(s => s > 0).WithMessage("所属backlog不能为空");
            RuleFor(createTask => createTask.assignedEmail).NotNull().WithMessage("负责人邮箱不能为空")
                .EmailAddress().WithMessage("负责人邮箱格式不正确").EmailAddress().WithMessage("负责人邮箱格式不正确"); ;
            RuleFor(createTask => createTask.state).Must(s => TaskState.CheckState(s)).WithMessage("状态编号不正确");
        }
    }
}
