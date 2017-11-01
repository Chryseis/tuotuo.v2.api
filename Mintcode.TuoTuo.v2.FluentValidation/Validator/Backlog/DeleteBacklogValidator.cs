using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.Backlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.Backlog
{
    public class DeleteBacklogValidator : AbstractValidator<ReqDeleteBacklog>
    {
        public DeleteBacklogValidator()
        {
            RuleFor(delete => delete.backlogID).Must(s => s > 0).WithMessage("backlog ID 不能为空");
        }
    }
}
