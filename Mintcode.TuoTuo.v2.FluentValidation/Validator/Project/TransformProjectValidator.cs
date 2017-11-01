using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class TransformProjectValidator : AbstractValidator<ReqTransformProject>
    {
        public TransformProjectValidator()
        {
            RuleFor(transform => transform.mail).NotNull().NotEmpty().WithMessage("转交用户邮箱不能为空").EmailAddress().WithMessage("转交用户邮箱格式不正确");
        }
    }
}
