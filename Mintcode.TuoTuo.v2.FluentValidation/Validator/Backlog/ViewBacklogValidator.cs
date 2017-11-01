using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class ViewBacklogValidator : AbstractValidator<ReqViewBacklog>
    {
        public ViewBacklogValidator()
        {
            RuleFor(viewBacklog => viewBacklog.backlogID).Must(s => s > 0).WithMessage("Backlog ID 不能为空");
        }
    }
}
