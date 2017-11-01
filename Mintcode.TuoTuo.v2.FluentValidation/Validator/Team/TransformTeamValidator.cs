using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.Team;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.Team
{
    public class TransformTeamValidator : AbstractValidator<ReqTransformTeam>
    {
        public TransformTeamValidator()
        {
            RuleFor(transform => transform.mail).NotNull().WithMessage("转交用户邮箱不能为空").EmailAddress().WithMessage("转交用户邮箱格式不正确");
        }
    }
}
