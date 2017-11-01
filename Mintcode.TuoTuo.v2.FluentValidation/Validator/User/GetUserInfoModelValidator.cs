using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator
{
    public class GetUserInfoModelValidator : AbstractValidator<ReqGetUserInfoModel>
    {
        public GetUserInfoModelValidator()
        {
            RuleFor(getUserInfoModel => getUserInfoModel.mail).NotNull().NotEmpty().WithMessage("邮箱不能为空").EmailAddress().WithMessage("邮箱格式不正确");
        }
    }
}
