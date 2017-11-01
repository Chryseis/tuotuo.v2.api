using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class InviteMemberValidator : AbstractValidator<ReqInviteMember>
    {
        public InviteMemberValidator()
        {
            RuleForEach(inviteMember => inviteMember.mails).NotNull().NotEmpty().WithMessage("邀请者邮箱不能为空").EmailAddress().WithMessage("邀请者邮箱格式不正确");
        }
    }
}
