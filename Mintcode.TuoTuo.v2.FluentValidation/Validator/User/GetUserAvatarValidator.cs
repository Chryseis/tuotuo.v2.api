﻿using FluentValidation;
using Mintcode.TuoTuo.v2.Webapi.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation.Validator.User
{
    public class GetUserAvatarValidator : AbstractValidator<ReqGetUserAvatar>
    {
        public GetUserAvatarValidator()
        {
            RuleFor(getUserAvatar => getUserAvatar.selectUserMail).NotNull().NotEmpty().WithMessage("邮箱不能为空").EmailAddress().WithMessage("邮箱格式不正确");    
        }
    }
}
