using Mintcode.TuoTuo.v2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Identity.CAS
{
    public class CASOAuthHandler : BaseOAuthHandler
    {

        //private string client_id= "Test";

        //private string client_secret= "21B";

        //private string redirect_uri= "http://localhost:4977/cas";

        //private string cas_oauth_token_uri = "http://192.168.4.110:8080/cas/oauth2.0/accessToken";

        //private string cas_oauth_profile_uri = "http://192.168.4.110:8080/cas//oauth2.0/profile";

        protected override string client_id
        {
            get
            {
                return ConfigurationManager.AppSettings["cas_client_id"];
            }
        }

        protected override string client_secret
        {
            get
            {
                return ConfigurationManager.AppSettings["cas_client_secret"];
            }
        }

        protected override string redirect_uri
        {
            get
            {
                return ConfigurationManager.AppSettings["cas_client_redirect_uri"];
            }
        }

        protected override string oauth_token_uri
        {
            get
            {
                return ConfigurationManager.AppSettings["cas_oauth_token_uri"];
            }
        }

        protected override string oauth_profile_uri
        {
            get
            {
                return ConfigurationManager.AppSettings["cas_oauth_profile_uri"];
            }
        }

        protected override string app_type
        {
            get
            {
                return "CAS";
            }
        }

        protected async override Task<string> AuthenticateWithThirdPartyApp(HttpRequestMessage request)
        {

            //从Url中获取授权码
            string authorizationCode = request.GetQueryNameValuePairs().Last(s => s.Key.Equals("code") && !string.IsNullOrEmpty(s.Value)).Value;

            string redirectUri = request.GetQueryNameValuePairs().Last(s => s.Key.Equals("redirectUri") && !string.IsNullOrEmpty(s.Value)).Value;

            //根据授权码获取Token
            EnhancedUriBuilder uriBuilder = new EnhancedUriBuilder(oauth_token_uri);
            uriBuilder.QueryItems.Add("client_id", client_id);
            uriBuilder.QueryItems.Add("redirect_uri", Uri.EscapeUriString(redirectUri));
            uriBuilder.QueryItems.Add("client_secret", client_secret);
            uriBuilder.QueryItems.Add("code", authorizationCode);
            string accessTokenUrl = uriBuilder.ToString();
            string accessTokenResponse = await Client.GetStringAsync(accessTokenUrl);
            List<KeyValuePair<string, string>> items = CreateItemsFromUriQuery(accessTokenResponse);
            string accessToken = items.SingleOrDefault(s => s.Key.Equals("access_token")).Value;

            //根据Token获取用户登录名
            uriBuilder = new EnhancedUriBuilder(oauth_profile_uri);
            uriBuilder.QueryItems.Add("access_token", accessToken);
            string profileUrl = uriBuilder.ToString();
            string profileResponse = await Client.GetStringAsync(profileUrl);
            ProfileModel profile = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileModel>(profileResponse);
            string id = profile.id;

            return id;
        }


    }
}
