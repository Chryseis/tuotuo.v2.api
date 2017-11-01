using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class SearchProjectByMailValidator : AbstractValidator<ReqSearchProjectByMail>
    {
        public SearchProjectByMailValidator()
        {
            RuleFor(searchProjectByMail => searchProjectByMail.mail).NotNull().NotEmpty().WithMessage("邮箱不能为空").EmailAddress().WithMessage("邮箱格式不正确");
        }
    }
}
