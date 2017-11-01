using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using log4net;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using System.Web.Http;
using System.Security.Claims;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Models.User;
using Mintcode.TuoTuo.v2.Repository;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    [RoutePrefix("user")]
    public class UserController:BaseController
    {
        private IUserRepository _userRepo;

        private IRelationAccountRepository _relationRepo;

        private ITeamRepository _teamRepo;

        private IProjectRepository _projectRepo;

        private ILog _logger;

        public UserController(IUserRepository userRepo, 
            [KeyFilter("GlobalLog")] ILog logger, 
            IdentityService identityService,
            IRelationAccountRepository relationRepo,
            ITeamRepository teamRepo,
            IProjectRepository projectRepo
            )
            : base(identityService)
        {
            this._userRepo = userRepo;
            this._relationRepo = relationRepo;
            this._teamRepo = teamRepo;
            this._projectRepo = projectRepo;        
            this._logger = logger;
        }

        
        /// <summary>
        /// 请求验证码图片
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [Route("GetAuthPic")]
        [HttpGet]
        public IHttpActionResult GetAuthPic(string identity,int width,int height)
        {           
            var img = _userRepo.CreateAuthCode(identity, width, height);
            var contentType = "image/png";
            return new FileResult(img, contentType);
        }


        /// <summary>
        /// 注册时验证邮箱是否已经被注册（检验唯一性）
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [Route("VerifyEmail")]
        [HttpGet]
        public IHttpActionResult VerifyEmail(string mail)
        {
            var response = new ResponseBaseModel();
            var ret = _userRepo.VerifyEmail(mail);
            if (!ret)
            {
                Enforce.Throw(new LogicErrorException("当前邮箱已被注册"));     
            }          
            response.SetResponse(ResStatusCode.OK, "不存在");
            return Ok(response);
        }


        /// <summary>
        /// 发送邮件(验证码)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("SendAuthMail")]
        [HttpPost]
        public async Task<IHttpActionResult> SendAuthMail([FromBody]ReqSendAuthMail req)
        {
            var response = new ResSendAuthMail();
            string agent = Request.Headers.UserAgent.ToString();
                       
            //验证码验证成功或者重发token验证成功            
            var reSendToken = await this._userRepo.SendAuthMail(req.reSendEmailToken, req.identity, req.code, req.mail, req.codeType, agent);
            if (string.IsNullOrEmpty(reSendToken))
            {
                Enforce.Throw(new LogicErrorException("邮件发送失败"));   
            }

            //发送邮件成功   
            SendAuthMailResult sendAuthMailResult = new SendAuthMailResult();
            sendAuthMailResult.mail = req.mail;
            sendAuthMailResult.reSendMailToken = reSendToken;
            response.setResponse(ResStatusCode.OK, sendAuthMailResult, 1, "邮件发送成功");

            return Ok(response);
        }


        /// <summary>
        /// 验证邮箱验证码（生成秘钥返回）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("VerifyEmailAuthCode")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyEmailAuthCode(ReqVerifyEmailAuthCode req)
        {
            var response = new ResVerifyEmailAuthCode();
            string agent = Request.Headers.UserAgent.ToString();

            var submitToken = await _userRepo.VerifyEmailAuthCode(req.mail, req.codeType, agent, req.code);
            if (string.IsNullOrEmpty(submitToken))
            {
                Enforce.Throw(new LogicErrorException("验证失败"));
            }     

            VerifyEmailAuthCodeResult verifyEmailAuthCodeResult = new VerifyEmailAuthCodeResult();
            verifyEmailAuthCodeResult.mail = req.mail;
            verifyEmailAuthCodeResult.submitToken = submitToken;
            response.setResponse(ResStatusCode.OK, verifyEmailAuthCodeResult, 1, "验证成功");
            return Ok(response);
        }



        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("RegisterUser")]
        [HttpPost]
        public async Task<HttpResponseMessage> RegisterUser(ReqUserRegister req)
        {
            string agent = Request.Headers.UserAgent.ToString();
            var userRepoModel =await _userRepo.RegisterUser(req.submitToken,req.redisId,req.mail,req.password,req.name, agent);
            if (userRepoModel==null)
            {
                Enforce.Throw(new LogicErrorException("注册失败"));
            }
            this._projectRepo.UpdateProjectMemberApproveState(req.mail);
            this._teamRepo.UpdateTeamMemberApproveState(req.mail);
            UserClaimsInfoModel userClaimsInfoModel = this._identityService.CreateUserClaimsInfoModelFromUserRepoModel(userRepoModel);
            var responseOAuth = await _identityService.OAuthSuccessResponse(userClaimsInfoModel);
            return responseOAuth;
        }


        /// <summary>
        /// 第三方账号登录成功之后绑定现有账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("BindUser")]
        [HttpPost]
        public async Task<HttpResponseMessage> BindUser(ReqBindUser req)
        {
            string agent = Request.Headers.UserAgent.ToString();
            var userRepoModel =await _userRepo.BindUser(req.submitToken,req.redisId,req.mail, agent);
            if (userRepoModel==null)
            {
                Enforce.Throw(new LogicErrorException("绑定失败"));
            }
            UserClaimsInfoModel userClaimsInfoModel = this._identityService.CreateUserClaimsInfoModelFromUserRepoModel(userRepoModel);
            var responseOAuth = await _identityService.OAuthSuccessResponse(userClaimsInfoModel);
            return responseOAuth;
        }

        /// <summary>
        /// 第三方账号登录成功之后绑定现有账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("BindUserV2")]
        [HttpPost]
        public async Task<HttpResponseMessage> BindUserV2(ReqBindUserV2 req)
        {
            var userRepoModel = await _userRepo.BindUserV2(req.redisId, req.mail, req.password);
            if (userRepoModel == null)
            {
                Enforce.Throw(new LogicErrorException("绑定失败"));
            }
            UserClaimsInfoModel userClaimsInfoModel = this._identityService.CreateUserClaimsInfoModelFromUserRepoModel(userRepoModel);
            var responseOAuth = await _identityService.OAuthSuccessResponse(userClaimsInfoModel);
            return responseOAuth;
        }



        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ResetPassword(ReqResetPassword req)
        {
            string agent = Request.Headers.UserAgent.ToString();
            var response = new ResponseBaseModel();
            if (!await this._userRepo.ResetPassword(req.submitToken, req.mail,req.password, agent))
            {
                Enforce.Throw(new LogicErrorException("重置失败"));  
            }
          
            response.SetResponse(ResStatusCode.OK, "重置成功");
            return Ok(response);
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ChangePassword")]
        [HttpPost]
        [CustomAuthorize]
        public IHttpActionResult ChangePassword(ReqChangePassword req)
        {
            var response = new ResponseBaseModel();
            if (!this._userRepo.ChangePassword(req.mail, req.oldPassword, req.newPassword))
            {
                Enforce.Throw(new LogicErrorException("修改失败"));
            }     
            response.SetResponse(ResStatusCode.OK, "修改成功");
            return Ok(response);

        }



        /// <summary>
        /// 当前用户的信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("MyUserInfo")]
        [HttpGet]
        [CustomAuthorize]
        public IHttpActionResult MyUserInfo()
        {
            UserClaimsInfoModel userClaimsInfoModel=this.GetUserModelFromCurrentClaimsIdentity();
            UserRepoModel userRepoModel= this._userRepo.GetUser(userClaimsInfoModel.mail,1);
            ResMyUserInfo response = new ResMyUserInfo();
            response.setResponse(ResStatusCode.OK, userRepoModel,1);
            return Ok(response);

        }


        /// <summary>
        /// 获取相应的用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("GetUserInfo")]
        [HttpGet]
        [CustomAuthorize]
        public IHttpActionResult GetUserInfo(ReqGetUserInfoModel req)
        {
            ResMyUserInfo response = new ResMyUserInfo();
            UserRepoModel userRepoModel = this._userRepo.GetUser(req.mail);
            if (userRepoModel==null)
            {
                Enforce.Throw(new LogicErrorException("用户不存在"));
            }
            response.setResponse(ResStatusCode.OK, userRepoModel, 1);
            return Ok(response);
        }

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="selectUserName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [Route("GetUserAvatar")]
        [HttpGet]
        public IHttpActionResult GetUserAvatar([FromUri]ReqGetUserAvatar req)
        {
            if (req.width <= 0)
            {
                req.width = 100;
            }
            if (req.height <= 0)
            {
                req.height = 100;
            }
            var img = _userRepo.GetUserAvatar(req.selectUserMail,req.width,req.height);
            string contentType = AttachmentHelper.TransformExtensionToContentType(img.Item1);
            return new FileResult(img.Item2, contentType);
        }


        /// <summary>
        /// 修改个人消息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ModifyMyUserInfo")]
        [HttpPost]
        public async Task<IHttpActionResult> ModifyMyUserInfo([FromBody]ReqModifyMyUserInfo req)
        {
            var response = new ResponseBaseModel();
            UserClaimsInfoModel userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            if (!await this._userRepo.ModifyMyUserInfo(userClaimsInfoModel.mail, req.userName, req.avatarToken,req.mobile))
            {
                Enforce.Throw(new LogicErrorException("修改失败"));    
            }
            response.SetResponse(ResStatusCode.OK, "修改成功");
            return Ok(response);
        }
    }
}
