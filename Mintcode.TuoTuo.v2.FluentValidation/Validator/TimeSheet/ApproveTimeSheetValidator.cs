using FluentValidation;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.TimeSheet
{
   public class ApproveTimeSheetValidator : AbstractValidator<ReqApproveTimeSheet>
    {
        public ApproveTimeSheetValidator()
        {
            RuleFor(approve => approve.sheetID).Must(s => s > 0).WithMessage("TimeSheet ID不能为空");

            RuleFor(approve => approve.result).Must(result => Enum.IsDefined(typeof(TimeSheetResultStatus), result)).WithMessage("不存在当前审批操作");
        }
    }
}
