using Microsoft.Owin.Security.OAuth;
using Mintcode.TuoTuo.v2.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Identity
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private IUserRepository userRepository;
        private IdentityService identityService;
        public SimpleAuthorizationServerProvider(IUserRepository _userRepository, IdentityService _identityService)
        {
            userRepository = _userRepository;
            identityService = _identityService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();

            await base.ValidateClientAuthentication(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (string.IsNullOrEmpty(context.UserName))
            {
                context.SetError(ErrorOAuthCode.Invalid_Email, "邮箱不能为空");
                return;
            }

            if (string.IsNullOrEmpty(context.Password))
            {
                context.SetError(ErrorOAuthCode.Invalid_Password, "密码不能为空");
                return;
            }

            //从数据库获取用户和角色信息
            bool loginResult = userRepository.LoginApp(context.UserName, context.Password, null, null);
            if (!loginResult)
            {
                context.SetError(ErrorOAuthCode.Invalid_Account, "邮箱或者密码不正确");
                return;
            }
            var userRole = userRepository.GetUser(context.UserName);         
            UserClaimsInfoModel userClaimsInfoModel = this.identityService.CreateUserClaimsInfoModelFromUserRepoModel(userRole);
            var claimsIdentity = identityService.CreateClaimsIdentity(context.Options.AuthenticationType, userClaimsInfoModel);
           
            var authenticationTicket = identityService.CreateAuthenticationTicket(claimsIdentity);
            context.Validated(authenticationTicket);

            await base.GrantResourceOwnerCredentials(context);

        }



    }
}
