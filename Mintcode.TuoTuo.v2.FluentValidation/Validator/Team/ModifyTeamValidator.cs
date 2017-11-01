using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using Mintcode.TuoTuo.v2.Webapi.Models.Team;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.Team
{
    public class ModifyTeamValidator : AbstractValidator<ReqModifyTeam>
    {
        public ModifyTeamValidator()
        {
            RuleFor(modify => modify.teamName).NotNull().WithMessage("团队名称不能为空");
        }
    }
}
