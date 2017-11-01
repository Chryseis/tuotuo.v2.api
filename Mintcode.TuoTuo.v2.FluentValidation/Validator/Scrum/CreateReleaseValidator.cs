using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class CreateReleaseValidator : AbstractValidator<ReqCreateRelease>
    {
        public CreateReleaseValidator()
        {
            RuleFor(createRelease => createRelease.releaseName).NotNull().NotEmpty().WithMessage("Release名称不能为空");
        }
    }
}
