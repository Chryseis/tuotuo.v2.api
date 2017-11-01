using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.Scrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class DeleteSprintValidator : AbstractValidator<ReqDeleteSprint>
    {
        public DeleteSprintValidator()
        {
            RuleFor(deleteSprint => deleteSprint.sprintID).Must(s => s > 0).WithMessage("Sprint ID不能为空");
        }
    }
}
