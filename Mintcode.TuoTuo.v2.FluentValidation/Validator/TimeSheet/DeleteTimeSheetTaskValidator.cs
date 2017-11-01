using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.TimeSheet
{
   public class DeleteTimeSheetTaskValidator : AbstractValidator<ReqDeleteTimeSheetTask>
    {
        public DeleteTimeSheetTaskValidator()
        {
            RuleFor(delete => delete.taskID).Must(s => s > 0).WithMessage("TimeSheet Task ID不能为空");
        }
    }
}
