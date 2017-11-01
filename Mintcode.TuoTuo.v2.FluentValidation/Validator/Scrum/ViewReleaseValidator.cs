using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class ViewReleaseValidator : AbstractValidator<ReqViewRelease>
    {
        public ViewReleaseValidator()
        {
            RuleFor(viewRelease => viewRelease.releaseID).Must(s=>s>0).WithMessage("Release ID 不能为空");
        }
    }
}
