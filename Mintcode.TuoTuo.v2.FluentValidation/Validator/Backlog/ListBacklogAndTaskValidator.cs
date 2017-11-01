using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.Backlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.Backlog
{
    public class ListBacklogAndTaskValidator : AbstractValidator<ReqListBacklogAndTask>
    {
        public ListBacklogAndTaskValidator()
        {
            RuleFor(listBacklogAndTask => listBacklogAndTask.sprintID).Must(s => s > 0).WithMessage("Sprint ID不能为空");
        }
    }
}
