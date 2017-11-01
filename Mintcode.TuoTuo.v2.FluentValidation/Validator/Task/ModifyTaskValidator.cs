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
    public class ModifyTaskValidator : AbstractValidator<ReqModifyTask>
    {
        public ModifyTaskValidator()
        {
            RuleFor(modifyTask => modifyTask.state).Must(s => TaskState.CheckState(s)).WithMessage("状态编号不正确");
        }
    }
}
