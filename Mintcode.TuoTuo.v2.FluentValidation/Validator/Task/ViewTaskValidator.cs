using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using Mintcode.TuoTuo.v2.Webapi.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.Task
{
    public class ViewTaskValidator : AbstractValidator<ReqViewTask>
    {
        public ViewTaskValidator()
        {
            RuleFor(viewTask => viewTask.taskID).Must(s => s > 0).WithMessage("Task ID 不能为空");
        }
    }
}
