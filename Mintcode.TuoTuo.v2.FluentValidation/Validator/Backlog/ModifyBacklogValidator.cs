using FluentValidation;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class ModifyBacklogValidator : AbstractValidator<ReqModifyBacklog>
    {
        public ModifyBacklogValidator()
        {
            RuleFor(modifyBacklog=> modifyBacklog.backlogID).Must(s=>s>0).WithMessage("backlog ID 不能为空");
            RuleFor(modifyBacklog => modifyBacklog.title).NotNull().NotEmpty().WithMessage("Backlog标题不能为空");
            RuleFor(modifyBacklog => modifyBacklog.selectProjectID).Must(s => s > 0).WithMessage("所属项目不能为空");
            RuleFor(modifyBacklog => modifyBacklog.assignUserMail).NotNull().NotEmpty().WithMessage("负责人邮箱不能为空")
                .EmailAddress().WithMessage("负责人邮箱格式不正确");
            RuleFor(modifyBacklog => modifyBacklog.state).Must(s => BacklogState.CheckState(s)).WithMessage("状态编号不正确");
        }
    }
}
