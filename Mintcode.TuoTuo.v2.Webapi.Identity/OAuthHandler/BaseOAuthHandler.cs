using Microsoft.Owin.Security.OAuth;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Identity
{
    public abstract class BaseOAuthHandler : DelegatingHandler
    {        
        private IUserRepository userRepository;

        private IRelationAccountRepository relationAccountRepository;

        private IdentityService identityService;

        protected static readonly HttpClient Client;


        static BaseOAuthHandler()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Connection.Add("keep-alive");
        }


        #region 属性

        protected abstract string client_id { get; }

        protected abstract string client_secret { get; }

        protected abstract string redirect_uri { get; }

        protected abstract string oauth_token_uri { get; }

        protected abstract string oauth_profile_uri { get; }
        protected abstract string app_type { get; }

        #endregion

        #region 方法


        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            string id = string.Empty;

            try
            {
                id = await AuthenticateWithThirdPartyApp(request);
            }
            catch (Exception ex)
            {
                ErrorOAtuhModel res = new ErrorOAtuhModel();
                res.error = ErrorOAuthCode.Invalid_ThirdParty_OAuth;
                res.error_description = "第三方App认证失败";
                response = identityService.GenerateOAuthRespose(res);
                return response;
            }
            bool loginRessult = userRepository.LoginApp(null, null, id, this.app_type);
            if (loginRessult)
            {
                var userRole = userRepository.GetUserByThirdPartyId(id, this.app_type);

                #region 当前第三方用户已经在本应用绑定过

                //根据绑定获取用户名和角色
                
                UserClaimsInfoModel userClaimsInfoModel = this.identityService.CreateUserClaimsInfoModelFromUserRepoModel(userRole);
                response = await identityService.OAuthSuccessResponse(userClaimsInfoModel);

                #endregion
            }
            else
            {
                #region 当前第三方用户尚未在本应用绑定过

                UnBindErrorOAuthModel res = new UnBindErrorOAuthModel();
                res.error = ErrorOAuthCode.Invalid_bind_OAuth;
                res.error_description = "第三方App认证成功，但尚未与本地账号绑定";
                //要存储到redis中
                string relationAccountToken = Guid.NewGuid().ToString();
                RelationAccountModel relationAccountModel = new RelationAccountModel();
                relationAccountModel.from = this.app_type;
                relationAccountModel.thirdPartyID = id;
                await relationAccountRepository.InsertRelationAccountModel(relationAccountToken, relationAccountModel, new TimeSpan(1, 0, 0));
                res.relationAccountID = relationAccountToken;
                response = identityService.GenerateOAuthRespose(res);


                #endregion
            }


            return response;
        }



        /// <summary>
        ///将Url中Query形式的字符串转换成键值对的列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected List<KeyValuePair<string, string>> CreateItemsFromUriQuery(string query)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            string[] items = query.Split('&');

            foreach (string item in items)
            {
                if (item.Length > 0)
                {
                    string[] namevalue = item.Split('=');
                    if (list.Select(s => s.Key.Equals(namevalue[0])).Count() > 0)
                    {
                        list.RemoveAll(s => s.Key.Equals(namevalue[0]));
                    }
                    list.Add(new KeyValuePair<string, string>(namevalue[0], namevalue.Length > 1 ? namevalue[1] : String.Empty));
                }
            }
            return list;
        }


        /// <summary>
        /// 设置重定向Url的应答
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected HttpResponseMessage SetRedirectResponse(string uri)
        {
            Enforce.ArgumentNotNull(uri, "The Redirect Url cannot be null.");

            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Redirect;
            response.Headers.Location = new Uri(uri);
            return response;
        }



        /// <summary>
        /// 第三方App认证
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract Task<string> AuthenticateWithThirdPartyApp(HttpRequestMessage request);



        public void SetIdentityService(IdentityService _identityService)
        {
            this.identityService = _identityService;
        }

        public void SetUserRepository(IUserRepository _userRepository)
        {
            this.userRepository = _userRepository;
        }

        public void SetRelationAccountRepository(IRelationAccountRepository _relationAccountRepository)
        {
            this.relationAccountRepository = _relationAccountRepository;
        }


        /// <summary>
        /// 从请求中获取Url(不包含请求路径和参数)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetOAuthRedirectUrl(HttpRequestMessage request)
        {
            string url = string.Format("{0}://{1}", request.RequestUri.Scheme, request.RequestUri.Authority);
            if (!string.IsNullOrEmpty(this.redirect_uri) && this.redirect_uri.StartsWith("/"))
            {
                url = string.Concat(url, this.redirect_uri);
            }
            else if (!string.IsNullOrEmpty(this.redirect_uri))
            {
                url = string.Concat(url, "/", this.redirect_uri);
            }
            return url;
        }

        #endregion



    }
}
