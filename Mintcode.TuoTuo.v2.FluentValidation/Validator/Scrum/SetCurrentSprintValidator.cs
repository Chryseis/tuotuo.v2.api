using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.Scrum
{
    public class SetCurrentSprintValidator : AbstractValidator<ReqSetCurrentSprint>
    {
        public SetCurrentSprintValidator()
        {
            RuleFor(setCurrentSprint => setCurrentSprint.sprintID).Must(s => s > 0).WithMessage("Sprint ID不能为空");
        }
    }
}
