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
    public class CreateBacklogValidator : AbstractValidator<ReqCreateBacklog>
    {
        public CreateBacklogValidator()
        {
            RuleFor(createBacklog => createBacklog.title).NotNull().NotEmpty().WithMessage("Backlog标题不能为空");
            RuleFor(createBacklog => createBacklog.selectProjectID).Must(s=>s>0).WithMessage("所属项目不能为空");
            RuleFor(createBacklog => createBacklog.assignUserMail).NotNull().NotEmpty().WithMessage("负责人邮箱不能为空")
                .EmailAddress().WithMessage("负责人邮箱格式不正确");
            RuleFor(createBacklog => createBacklog.state).Must(s => BacklogState.CheckState(s)).WithMessage("状态编号不正确");

        }
    }
}
