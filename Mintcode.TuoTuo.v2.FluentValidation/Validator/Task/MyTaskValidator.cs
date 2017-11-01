using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.Task
{
    public class MyTaskValidator : AbstractValidator<ReqMyTask>
    {
        public MyTaskValidator()
        {
            //RuleFor(myTask => myTask.Date)
            //        .NotNull().NotEmpty().WithMessage("日期不能为空")
            //        .Must(ValidateHelper.BeAValidDate).WithMessage("日期格式不正确");
        }
    }
}
