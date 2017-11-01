using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.TuoTuo.v2.Model;
using System.IO;
using Mintcode.TuoTuo.v2.Common;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="mail">邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        Task<UserRepoModel> RegisterUser(string submitToken, string redisId, string mail, string password, string name, string agent);

        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="mail">邮箱</param>
        /// <param name="thirdPartyId">第三方id</param>
        /// <param name="from">来源</param>
        /// <param name="roles">角色</param>
        /// <returns></returns>
        Task<UserRepoModel> BindUser(string submitToken, string redisId, string mail, string agent);

        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="redisId"></param>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<UserRepoModel> BindUserV2(string redisId, string mail, string password);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="mail">邮箱</param>
        /// <returns></returns>
        UserRepoModel GetUser(string mail, int? status = 1, string password = null);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="thirdPartyId"></param>
        /// <returns></returns>
        UserRepoModel GetUserByThirdPartyId(string thirdPartyId,string from);


        /// <summary>
        /// 登录app
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <param name="thirdPartyId"></param>
        /// <param name="from"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool LoginApp(string mail, string password, string thirdPartyId, string from);

        /// <summary>
        /// 创建验证码图片
        /// </summary>
        /// <returns></returns>
        Stream CreateAuthCode(string identity,int width,int height);



        /// <summary>
        /// 发送验证邮件
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        Task<string> SendAuthMail(string reSendEmailToken, string identity, string code, string mail, int codeType, string agent);


        /// <summary>
        /// 验证邮箱密码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<string> VerifyEmailAuthCode(string mail, int codeType, string agent, string code);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPasssword"></param>
        /// <returns></returns>
        bool ChangePassword(string mail, string oldPassword, string newPasssword);


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ResetPassword(string submitToken, string mail, string password, string agent);

        /// <summary>
        /// 验证邮箱是否被注册
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        bool VerifyEmail(string mail);


        /// <summary>
        /// 根据邮箱列表获取用户信息列表
        /// </summary>
        /// <param name="mails"></param>
        /// <returns></returns>
        List<UserInfoModel> GetUserInfoModelList(List<string> mails);


        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="selectUserMail"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        Tuple<string, Stream> GetUserAvatar(string selectUserMail,int width,int height);


        /// <summary>
        /// 更改用户信息
        /// </summary>
        /// <param name="currentUserMail"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<bool> ModifyMyUserInfo(string currentUserMail, string userName, string avatarToken, string mobile);

    }
}
