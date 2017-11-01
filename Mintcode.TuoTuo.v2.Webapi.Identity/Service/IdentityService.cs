using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Identity
{
    public class IdentityService
    {
        public OAuthAuthorizationServerOptions options { set; get; }
        public IdentityService()
        {

        }
        public async Task<HttpResponseMessage> OAuthSuccessResponse(UserClaimsInfoModel userClaimsInfoModel)
        {
            DateTimeOffset currentUtc = DateTimeOffset.UtcNow;
            DateTimeOffset expiresUtc = currentUtc.Add(options.AccessTokenExpireTimeSpan);
            var claimsIdentity = CreateClaimsIdentity(options.AuthenticationType, userClaimsInfoModel);
            var ticket = CreateAuthenticationTicket(claimsIdentity, null, currentUtc, expiresUtc);
            string accessToken = options.AccessTokenFormat.Protect(ticket);

            AuthenticationTokenCreateContext authenticationTokenCreateContext = new AuthenticationTokenCreateContext(null,
                options.RefreshTokenFormat, ticket);
            await options.RefreshTokenProvider.CreateAsync(authenticationTokenCreateContext);
            string refreshToken = authenticationTokenCreateContext.Token;

            AccessTokenModel model = new AccessTokenModel();
            model.access_token = accessToken;
            model.refresh_token = refreshToken;
            model.token_type = "bearer";
            TimeSpan? expiresTimeSpan = expiresUtc - currentUtc;
            var expiresIn = (long)expiresTimeSpan.Value.TotalSeconds;
            if (expiresIn > 0)
            {
                model.expires_in = expiresIn;
            }
            return GenerateOAuthRespose(model);
        }

        //public static string CreateRefeshTokenAndSetTicketExpires(AuthenticationTicket ticket)
        //{
        //    string tokenValue = Guid.NewGuid().ToString("n");

        //    TimeSpan? expireTimeSpan = null;
        //    if (ticket.Properties.IssuedUtc.HasValue && ticket.Properties.ExpiresUtc.HasValue)
        //    {
        //        expireTimeSpan = ticket.Properties.ExpiresUtc.Value - ticket.Properties.IssuedUtc.Value;
        //        ticket.Properties.IssuedUtc = DateTimeOffset.UtcNow;
        //        ticket.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddTicks(expireTimeSpan.Value.Ticks * 2);
        //    }
        //    return tokenValue;
        //}

        /// <summary>
        /// 创建身份声明
        /// </summary>
        /// <param name="authenticationType"></param>
        /// <param name="userRepoModel"></param>
        /// <returns></returns>
        public ClaimsIdentity CreateClaimsIdentity(string authenticationType, UserClaimsInfoModel userClaimsInfoModel)
        {
            var claimsIdentity = new ClaimsIdentity(authenticationType);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userClaimsInfoModel.userID.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, userClaimsInfoModel.mail));
            return claimsIdentity;
        }

        /// <summary>
        /// 根据身份认证创建UserRepoModel对象
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns></returns>
        public UserClaimsInfoModel CreateUserClaimsInfoModelFromClaimsIdentity(ClaimsIdentity claimsIdentity)
        {
            UserClaimsInfoModel userClaimsInfoModel = new UserClaimsInfoModel();

            int userID = 0;
            int.TryParse(claimsIdentity.Name, out userID);
            userClaimsInfoModel.userID = userID;

            userClaimsInfoModel.mail = claimsIdentity.FindFirst(ClaimTypes.Email).Value;

            return userClaimsInfoModel;
        }

        public UserClaimsInfoModel CreateUserClaimsInfoModelFromUserRepoModel(UserRepoModel userRepoModel)
        {
            UserClaimsInfoModel userClaimsInfoModel = null;
            if (userRepoModel!=null)
            {
                userClaimsInfoModel = new UserClaimsInfoModel();
                userClaimsInfoModel.userID = userRepoModel.info.userID;
                userClaimsInfoModel.mail = userRepoModel.info.mail;
            }
            return userClaimsInfoModel;
        }


        /// <summary>
        /// 创建认证密钥
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="properties"></param>
        /// <param name="issuedUtc"></param>
        /// <param name="expiresUtc"></param>
        /// <returns></returns>
        public AuthenticationTicket CreateAuthenticationTicket(ClaimsIdentity claimsIdentity, IDictionary<string, string> properties = null,
            DateTimeOffset? issuedUtc = null, DateTimeOffset? expiresUtc = null)
        {
            var ticket = new AuthenticationTicket(claimsIdentity, new AuthenticationProperties(properties));
            ticket.Properties.IssuedUtc = issuedUtc;
            ticket.Properties.ExpiresUtc = expiresUtc;
            return ticket;
        }

        public HttpResponseMessage GenerateOAuthRespose(object model)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            responseMessage.Headers.Add("Cache-Control", "no-cache");
            responseMessage.Headers.Add("Pragma", "no-cache");

            responseMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(model));
            responseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            responseMessage.Content.Headers.ContentEncoding.Add("utf-8");
            responseMessage.Content.Headers.Expires = DateTimeOffset.MinValue;

            return responseMessage;
        }


        /// <summary>
        /// 将数据库本地的角色转化为OAuth Claim的角色
        /// </summary>
        /// <param name="localRoles"></param>
        /// <returns></returns>
        public List<string> TransformLocalRoleToOAuthRole(List<Role> localRoles)
        {
            List<string> oAuthRoles = new List<string>();
            if (null != localRoles && localRoles.Count > 0)
            {
                oAuthRoles = localRoles.Select(s => string.Concat(s.roleType, ":", s.relationID, ":", s.roleCode)).ToList();
            }
            return oAuthRoles;
        }


        /// <summary>
        /// 将OAuth Claim的角色 转化为数据库本地的角色
        /// </summary>
        /// <param name="oAuthRoles"></param>
        /// <returns></returns>
        public List<Role> TransformOAuthRoleToLocalRole(List<string> oAuthRoles)
        {
            List<Role> localRoles = new List<Role>();

            if (null != oAuthRoles && oAuthRoles.Count > 0)
            {
                foreach (var oAuthRole in oAuthRoles)
                {
                    List<string> array = oAuthRole.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (array.Count == 3)
                    {
                        Role role = new Role();
                        int relationID = 0;
                        RoleType roleType = RoleType.Team;
                        if (Enum.TryParse<RoleType>(array[0], true, out roleType) && int.TryParse(array[1], out relationID))
                        {
                            role.roleType = roleType;
                            role.relationID = relationID;
                            role.roleCode = array[2];
                            localRoles.Add(role);
                        }
                    }

                }
            }
            return localRoles;
        }

    }
}
