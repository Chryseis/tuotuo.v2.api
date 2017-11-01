using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Mintcode.Zeus.Public.Data;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.BLL;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.TuoTuo.v2.Infrastructure;
using StackExchange.Redis;
using Mintcode.TuoTuo.v2.Common;
using AutoMapper;
using Mintcode.TuoTuo.v2.Infrastructure.Util;

namespace Mintcode.TuoTuo.v2.Repository
{
    public class UserRepository : IUserRepository
    {
        private IDatabase _database;

        private IEmailService _mailService;

        private ITokenSecurity _tokenSecurity;

        private TuoTuoMailConfigurationSection _mailConfig;

        private T_USER_BLL _userBll;

        private T_THIRD_PARTY_BLL _thirdBll;

        private T_PROJECT_MEMBER_BLL _projectMemberbll;

        private T_TEAM_MEMBER_BLL _teamMemberbll;

        private IRelationAccountRepository _relationRepo;

        private TemplateHelper _templateHelper;

        private IFileService _fileService;

        private IAttachmentUploadRepository _attachmentUploadRepository;


        public UserRepository(IDatabase database,
            IEmailService mailService,
            ITokenSecurity tokenSecurity,
            TuoTuoMailConfigurationSection mailConfig,
            T_USER_BLL userBll,
            T_THIRD_PARTY_BLL thirdBll,
            T_PROJECT_MEMBER_BLL projectMemberbll,
            T_TEAM_MEMBER_BLL teamMemberbll,
            IRelationAccountRepository relationRepo,
            TemplateHelper templateHelper,
            IFileService fileService, 
            IAttachmentUploadRepository attachmentUploadRepository)
        {
            _database = database;
            _mailService = mailService;
            _tokenSecurity = tokenSecurity;
            _mailConfig = mailConfig;
            _userBll = userBll;
            _thirdBll = thirdBll;
            _projectMemberbll = projectMemberbll;
            _teamMemberbll = teamMemberbll;
            _relationRepo = relationRepo;
            _templateHelper = templateHelper;
            _fileService = fileService;
            _attachmentUploadRepository = attachmentUploadRepository;
        }

        public async Task<UserRepoModel> RegisterUser(string submitToken, string redisId, string mail, string password, string name, string agent)
        {
            UserRepoModel userRepoModel = null;

            if (await this.checkSubmitToken(EmailAuthCodeType.RegisterUser, mail, submitToken, agent))
            {
                var thirdParty = new RelationAccountModel();
                if (!string.IsNullOrEmpty(redisId))
                {
                    thirdParty = await _relationRepo.GetRelationAccountModel(redisId);
                    if (null == thirdParty)
                    {
                        Enforce.Throw(new FrontInputValidateErrorException("第三方Id不存在"));

                    }
                }

                var userInfoModel = new UserInfoModel();
                userInfoModel.userName = name;
                userInfoModel.userTrueName = name;
                userInfoModel.password = Encrypt.Base64Encode(password);
                userInfoModel.userLevel = 0;
                userInfoModel.sex = 0;
                userInfoModel.userStatus = 1;
                userInfoModel.lastLoginTime = DateTime.Now;
                bool ret = false;
                var userEntity = this.getUserInfoModelByMail(mail, null);
                //是否存在记录(当被邀请时会生成一条占位的记录)
                if (userEntity == null)
                {
                    userInfoModel.mail = mail.ToLower(); ;
                    userInfoModel.createTime = userInfoModel.lastLoginTime;
                    if (string.IsNullOrEmpty(name))
                    {
                        userInfoModel.userName = mail.Split('@')[0];
                        userInfoModel.userTrueName = mail.Split('@')[0];
                    }
                    var user = Mapper.Map<UserInfoModel, T_USER>(userInfoModel);
                    if (string.IsNullOrEmpty(thirdParty.thirdPartyID))
                    {
                        ret = this._userBll.Add(user);
                    }
                    else
                    {
                        if (this.getThirdPartyInfoModel(thirdParty.thirdPartyID, thirdParty.from) != null)
                        {
                            Enforce.Throw(new LogicErrorException("第三方账号已被其他账号绑定"));
                        }  
                        ret = this._userBll.AddUserAndThirdParty(user, thirdParty.thirdPartyID, thirdParty.from);                        
                    }
                    userInfoModel = Mapper.Map<T_USER, UserInfoModel>(user);
                }
                else if (userEntity != null && userEntity.userStatus == 0)
                {
                    userInfoModel.userID = userEntity.userID;
                    userInfoModel.mail = userEntity.mail;
                    userInfoModel.createTime = userEntity.createTime;
                    if (string.IsNullOrEmpty(name))
                    {
                        userInfoModel.userName = userEntity.userName;
                        userInfoModel.userTrueName = userEntity.userTrueName;
                    }
                    if (string.IsNullOrEmpty(thirdParty.thirdPartyID))
                    {
                        ret = this._userBll.Update(Mapper.Map<UserInfoModel, T_USER>(userInfoModel));
                    }
                    else
                    {
                        if (this.getThirdPartyInfoModel(thirdParty.thirdPartyID, thirdParty.from) != null)
                        {
                            Enforce.Throw(new LogicErrorException("第三方账号已被其他账号绑定"));
                        }
                        ret = this._userBll.UpdateUserAndThirdParty(Mapper.Map<UserInfoModel, T_USER>(userInfoModel), thirdParty.thirdPartyID, thirdParty.from);
                    }

                }


                if (ret)
                {
                    userRepoModel = new UserRepoModel();
                    userRepoModel.info = userInfoModel;
                    userRepoModel.roleList = this.getUserRoleListByUserID(userRepoModel.info.userID);
                }
            }

            return userRepoModel;
        }

        public async Task<UserRepoModel> BindUser(string submitToken, string redisId, string mail, string agent)
        {
            UserRepoModel userRepoModel = null;
            if (await this.checkSubmitToken(EmailAuthCodeType.BindUser, mail, submitToken, agent))
            {
                var thirdParty = await _relationRepo.GetRelationAccountModel(redisId);
                if (null == thirdParty)
                {
                    Enforce.Throw(new FrontInputValidateErrorException("第三方Id不存在"));
                }
                var userInfoModel = this.getUserInfoModelByMail(mail, 1);
                if (userInfoModel == null)
                {
                    Enforce.Throw(new LogicErrorException("当前账号不存在"));
                }

                if (this.getThirdPartyInfoModel(thirdParty.thirdPartyID, thirdParty.from) != null)
                {
                    Enforce.Throw(new LogicErrorException("第三方账号已被其他账号绑定"));
                }

                if (this.getThirdPartyInfoModel(userInfoModel.userID, thirdParty.from) != null)
                {
                    Enforce.Throw(new LogicErrorException("当前账号已绑定过该类型的第三方账号"));
                }

                userInfoModel.lastLoginTime = DateTime.Now;
                bool ret = this._userBll.UpdateUserAndThirdParty(Mapper.Map<UserInfoModel, T_USER>(userInfoModel), thirdParty.thirdPartyID, thirdParty.from);
                if (ret)
                {
                    userRepoModel = new UserRepoModel();
                    userRepoModel.info = userInfoModel;
                    userRepoModel.roleList = this.getUserRoleListByUserID(userRepoModel.info.userID);

                }
            }

            return userRepoModel;
        }


        public async Task<UserRepoModel> BindUserV2(string redisId, string mail, string password)
        {
            UserRepoModel userRepoModel = null;
            var thirdParty = await _relationRepo.GetRelationAccountModel(redisId);
            if (null == thirdParty)
            {
                Enforce.Throw(new FrontInputValidateErrorException("第三方Id不存在"));
            }
            var userInfoModel = this.getUserInfoModelByMailAndPassword(mail, password);
            if (userInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("当前账号不存在或密码错误"));
            }

            if (this.getThirdPartyInfoModel(thirdParty.thirdPartyID, thirdParty.from) != null)
            {
                Enforce.Throw(new LogicErrorException("第三方账号已被其他账号绑定"));
            }

            if (this.getThirdPartyInfoModel(userInfoModel.userID, thirdParty.from) != null)
            {
                Enforce.Throw(new LogicErrorException("当前账号已绑定过该类型的第三方账号"));
            }

            userInfoModel.lastLoginTime = DateTime.Now;
            bool ret = this._userBll.UpdateUserAndThirdParty(Mapper.Map<UserInfoModel, T_USER>(userInfoModel), thirdParty.thirdPartyID, thirdParty.from);
            if (ret)
            {
                userRepoModel = new UserRepoModel();
                userRepoModel.info = userInfoModel;
                userRepoModel.roleList = this.getUserRoleListByUserID(userRepoModel.info.userID);

            }

            return userRepoModel;
        }

        public UserRepoModel GetUser(string mail, int? status = 1, string password = null)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            UserRepoModel userRepoModel = null;
            var query = new DapperExQuery<T_USER>().AndWhere(t => t.U_EMAIL, OperationMethod.Equal, mail);
            if (!string.IsNullOrEmpty(password))
            {
                query.AndWhere(t => t.U_PASSWORD, OperationMethod.Equal, Encrypt.Base64Encode(password));
            }
            if (status != null)
            {
                query.AndWhere(k => k.U_STATUS, OperationMethod.Equal, 1);
            }
            var userEntity = this._userBll.GetEntity(query);
            if (null != userEntity)
            {
                userRepoModel = new UserRepoModel();
                userRepoModel.info = Mapper.Map<T_USER, UserInfoModel>(userEntity);
                userRepoModel.roleList = this.getUserRoleListByUserID(userEntity.ID);
            }
            return userRepoModel;
        }

        public UserRepoModel GetUserByThirdPartyId(string thirdPartyId, string from)
        {

            UserRepoModel userRepoModel = null;

            var thirdInfoModel = this.getThirdPartyInfoModel(thirdPartyId, from);
            if (thirdInfoModel != null)
            {
                var userInfoModel = this.getUserInfoModelByUserID(thirdInfoModel.userID, 1);
                userRepoModel = new UserRepoModel();
                userRepoModel.info = userInfoModel;
                userRepoModel.roleList = this.getUserRoleListByUserID(userInfoModel.userID); ;
            }
            return userRepoModel;
        }


        public bool LoginApp(string mail, string password, string thirdPartyId, string from)
        {

            bool ret = false;
            UserInfoModel userInfoModel = null;
            if (string.IsNullOrEmpty(thirdPartyId))
            {
                //正常登录
                userInfoModel=this.getUserInfoModelByMailAndPassword(mail, password);            
            }
            else
            {
                //第三方登录
                var thirdInfoModel = this.getThirdPartyInfoModel(thirdPartyId, from);
                if (null != thirdInfoModel)
                {
                    userInfoModel = this.getUserInfoModelByUserID(thirdInfoModel.userID,1);                      
                }

            }

            if (null != userInfoModel)
            {
                userInfoModel.lastLoginTime = DateTime.Now;
                ret = this._userBll.Update(Mapper.Map<UserInfoModel, T_USER>(userInfoModel));
            }

            return ret;
        }

        public Stream CreateAuthCode(string identity, int width, int height)
        {
            if (string.IsNullOrEmpty(identity))
            {
                Enforce.Throw(new FrontInputValidateErrorException("验证码identity不能为空"));
            }

            if (width < 0)
            {
                width = 100;
            }

            if (height < 0)
            {
                height = 40;
            }

            var authCode = AuthCode.CreateAuthCode(width, height, ImageFormat.Png);
            _database.StringSetAsync(identity, authCode.Code.ToLower(), TimeSpan.FromMinutes(5));
            return authCode.Img;
        }


        public async Task<string> SendAuthMail(string reSendEmailToken, string identity, string code, string mail, int codeType, string agent)
        {
            if (!Enum.IsDefined(typeof(EmailAuthCodeType), codeType))
            {
                Enforce.Throw(new FrontInputValidateErrorException("验证码邮件类型错误"));
            }
            EmailAuthCodeType mailCodeType = (EmailAuthCodeType)Enum.ToObject(typeof(EmailAuthCodeType), codeType);

            //判断是否包含重发token
            if (string.IsNullOrEmpty(reSendEmailToken))
            {
                if (string.IsNullOrEmpty(identity))
                {
                    Enforce.Throw(new FrontInputValidateErrorException("验证码identity不能为空"));
                }

                //去认证验证码，验证码通过才能发邮件
                var verifyAuthCodeResult = await this.verifyAuthCode(identity, code);
                if (!verifyAuthCodeResult)
                {
                    Enforce.Throw(new LogicErrorException("验证码错误"));
                }

            }
            else
            {
                //验证重发token，重发token通过才会发邮件
                bool verifyReSendMailToken = await this.verifyReSendAuthMailToken(mail, mailCodeType, agent, reSendEmailToken);
                if (!verifyReSendMailToken)
                {
                    Enforce.Throw(new LogicErrorException("重发邮件失败"));
                }
            }

            //验证码
            var authCode = AuthCode.CreateAuthCode(100, 40, ImageFormat.Png);

            var setCodeStatus = await _database.StringSetAsync(this.generateEmailCodeKey(mail, mailCodeType), authCode.Code.ToLower(), TimeSpan.FromMinutes(10));


            //重发token
            string token = this.generateToken(agent);
            var setTokenStatus = await _database.StringSetAsync(this.generateReSendMailTokenKey(mail, mailCodeType), token, TimeSpan.FromMinutes(20));


            if (setCodeStatus && setTokenStatus)
            {
                //异步发送邮件
                //_mailService.SendCodeMailAsync(mail, authCode.Code);
                //_mailService.SendMailAsync(mail,
                //    _mailConfig.CodeContent.Subject.Text,
                //    string.Format(_mailConfig.CodeContent.Body.Text, authCode.Code), true);
                //Todo:后续需要优化
                _mailService.SendMailAsync(mail,
                    _mailConfig.CodeContent.Subject.Text,
                     _templateHelper.GenerateContent(_mailConfig.CodeContent.Body.Text, "codeContent", new { Code = authCode.Code })
                     , true);

                return token;
            }


            return string.Empty;
        }


        public async Task<string> VerifyEmailAuthCode(string mail, int codeType, string agent, string code)
        {
            if (!Enum.IsDefined(typeof(EmailAuthCodeType), codeType))
            {
                Enforce.Throw(new FrontInputValidateErrorException("验证码邮件类型错误"));
            }
            EmailAuthCodeType mailCodeType = (EmailAuthCodeType)Enum.ToObject(typeof(EmailAuthCodeType), codeType);
            string emailCodeRedisKey = this.generateEmailCodeKey(mail, mailCodeType);
            var authCode = await _database.StringGetAsync(emailCodeRedisKey);
            if (authCode == code.ToLower())
            {
                //提交token
                string token = this.generateToken(agent);
                var status = await _database.StringSetAsync(this.generateSubmitTokenKey(mail, mailCodeType), token, TimeSpan.FromMinutes(10));
                if (status)
                {
                    _database.KeyDeleteAsync(emailCodeRedisKey);
                    _database.KeyDeleteAsync(this.generateReSendMailTokenKey(mail, mailCodeType));
                    return token;
                }
            }
            return string.Empty;

        }


        public async Task<bool> ResetPassword(string submitToken, string mail, string password, string agent)
        {
            bool result = true;
            if (await this.checkSubmitToken(EmailAuthCodeType.ResetPassword, mail, submitToken, agent))
            {
                result = this.editPassword(mail, password);
            }
            return result;
        }

        public bool ChangePassword(string mail, string oldPassword, string newPasssword)
        {
            if (this.GetUser(mail, 1, oldPassword) == null)
            {
                Enforce.Throw(new LogicErrorException("原密码错误"));
            }
            return this.editPassword(mail, newPasssword);
        }

        public bool VerifyEmail(string mail)
        {
            var userInfoModel = this.getUserInfoModelByMail(mail, 1);
            return userInfoModel == null;
        }

        public List<UserInfoModel> GetUserInfoModelList(List<string> mails)
        {
            if (mails != null && mails.Count > 0)
            {
                mails = mails.Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower()).ToList();
                var users = this._userBll.GetList(new DapperExQuery<T_USER>()
                                        .AndWhere(s => s.U_EMAIL, OperationMethod.In, mails.ToArray())
                                         .AndWhere(s => s.U_STATUS, OperationMethod.Equal, 1))
                                        .Select(s => Mapper.Map<T_USER, UserInfoModel>(s)).ToList();
                return users;
            }
            else
            {
                return new List<UserInfoModel>();
            }

        }

        public Tuple<string,Stream> GetUserAvatar(string selectUserMail, int width, int height)
        {
            var userInfoModel = this.getUserInfoModelByMail(selectUserMail,null);
            if (userInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户不存在"));
            }

            //存在头像
            byte[] bytes = null;
            string extension = string.Empty;
            if (!string.IsNullOrEmpty(userInfoModel.userAvatar))
            {              
                extension = Path.GetExtension(userInfoModel.userAvatar);              
                string baseFileNameWithoutExtension = userInfoModel.userAvatar.Remove(userInfoModel.userAvatar.LastIndexOf(extension));
                string fileID = baseFileNameWithoutExtension + "_" + width + "_" + height + extension;
                if (_fileService.CheckFileExist(fileID))
                {
                    bytes = _fileService.GetFile(fileID);
                }
                else
                {
                    var fileBytes = _fileService.GetFile(userInfoModel.userAvatar);
                    bytes = ImageUtils.CompressImage(fileBytes, extension, width, height, "HorW");
                    _fileService.UploadFile(fileID, bytes);
                }                    
            }
            else
            {
                #region 获取头像中的文字

                var avatarContent = string.Empty;
                string name = string.Empty;
                if (!string.IsNullOrEmpty(userInfoModel.userName))
                {
                    name = userInfoModel.userName;
                }
                else
                {
                    name = userInfoModel.mail.Split('@')[0];
                }
                if (name.Length >= 2)
                {
                    if ((int)name[0] > 127)
                    {
                        avatarContent = name[0].ToString();
                    }
                    else if ((int)name[0] <= 127 && (int)name[1] > 127)
                    {
                        avatarContent = name[1].ToString();
                    }
                    else
                    {
                        avatarContent = name[0].ToString() + name[1].ToString();
                    }
                }
                else
                {
                    avatarContent = name;
                }

                #endregion

                //Todo:需要优化
                extension = ".png";
                var fileBytes = Avatar.CreateAvatar(avatarContent);
                bytes = ImageUtils.CompressImage(fileBytes, extension, width, height, "HorW");
            }
            var tuple = new Tuple<string, Stream>(extension, new MemoryStream(bytes));
            return tuple;
        }

        public async Task<bool> ModifyMyUserInfo(string currentUserMail, string userName, string avatarToken,string mobile)
        {
            var currentUser = this.getUserInfoModelByMail(currentUserMail,1);
            if (currentUser == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户不存在"));
            }
            currentUser.userName = userName;
            currentUser.userTrueName = userName;         
            string userAvatar = string.Empty;
            if (!string.IsNullOrEmpty(avatarToken))
            {
                userAvatar = await _attachmentUploadRepository.GetAttachmentFileID(avatarToken, currentUserMail);
                currentUser.userAvatar = userAvatar;
            }
            currentUser.mobile = mobile;



            return this._userBll.Update(Mapper.Map<UserInfoModel,T_USER>(currentUser));
        }
        #region 私有方法

        /// <summary>
        /// Email 验证码的redis key
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        private string generateEmailCodeKey(string mail, EmailAuthCodeType codeType)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            return string.Concat(mail, ":", codeType.ToString(), ":", "code");

        }


        /// <summary>
        /// 重发邮件Token的redis key
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        private string generateReSendMailTokenKey(string mail, EmailAuthCodeType codeType)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            return string.Concat(mail, ":", codeType.ToString(), ":", "reSendToken");
        }

        /// <summary>
        /// 提交Token的redis key
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        private string generateSubmitTokenKey(string mail, EmailAuthCodeType codeType)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            return string.Concat(mail, ":", codeType.ToString(), ":", "submitToken");
        }


        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private string generateToken(string agent)
        {
            TokenContentModel tokenContentModel = new TokenContentModel();
            tokenContentModel.agent = agent;
            tokenContentModel.timestamp = DateTime.Now.ToTimeStamp();
            string token = _tokenSecurity.encrypt<TokenContentModel>(tokenContentModel);
            return token;
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private UserInfoModel getUserInfoModelByMail(string mail, int? status)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            UserInfoModel userInfoModel = null;
            var query = new DapperExQuery<T_USER>().AndWhere(t => t.U_EMAIL, OperationMethod.Equal, mail);
            if (status != null)
            {
                query.AndWhere(t => t.U_STATUS, OperationMethod.Equal, status.Value);
            }
            var userEntity = this._userBll.GetEntity(query);
            if (userEntity != null)
            {
                userInfoModel = Mapper.Map<T_USER, UserInfoModel>(userEntity);
            }
            return userInfoModel;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private UserInfoModel getUserInfoModelByUserID(int userID, int? status)
        {
            UserInfoModel userInfoModel = null;
            var query = new DapperExQuery<T_USER>().AndWhere(t => t.ID, OperationMethod.Equal, userID);
            if (status != null)
            {
                query.AndWhere(t => t.U_STATUS, OperationMethod.Equal, status.Value);
            }
            var userEntity = this._userBll.GetEntity(query);
            if (userEntity != null)
            {
                userInfoModel = Mapper.Map<T_USER, UserInfoModel>(userEntity);
            }
            return userInfoModel;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private UserInfoModel getUserInfoModelByMailAndPassword(string mail,string password)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            UserInfoModel userInfoModel = null;
            var query = new DapperExQuery<T_USER>().AndWhere(t => t.U_EMAIL, OperationMethod.Equal, mail)
                .AndWhere(t=>t.U_PASSWORD,OperationMethod.Equal, Encrypt.Base64Encode(password))
                .AndWhere(t => t.U_STATUS, OperationMethod.Equal, 1);        
            var userEntity = this._userBll.GetEntity(query);
            if (userEntity != null)
            {
                userInfoModel = Mapper.Map<T_USER, UserInfoModel>(userEntity);
            }
            return userInfoModel;
        }


        /// <summary>
        /// 根据用户ID获取用户全部的角色
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private List<Role> getUserRoleListByUserID(int userID)
        {
            List<Role> list = new List<Role>();

            //团队的角色
            var teamRoleList = this._teamMemberbll.GetList(new DapperExQuery<T_TEAM_MEMBER>().AndWhere(t => t.U_USER_ID, OperationMethod.Equal, userID))
                    .Select(t => Mapper.Map<T_TEAM_MEMBER, Role>(t))
                    .ToList();
            list.AddRange(teamRoleList);

            //项目的角色
            var projectRoleList = this._projectMemberbll.GetList(new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(t => t.U_USER_ID, OperationMethod.Equal, userID))
                   .Select(t => Mapper.Map<T_PROJECT_MEMBER, Role>(t))
                   .ToList();
            list.AddRange(projectRoleList);

            return list;
        }


        /// <summary>
        /// 获取第三方绑定信息
        /// </summary>
        /// <param name="thirdPartyId"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        private ThirdPartyInfoModel getThirdPartyInfoModel(string thirdPartyId, string from)
        {
            ThirdPartyInfoModel thirdPartyInfoModel = null;
            var query = new DapperExQuery<T_THIRD_PARTY>().AndWhere(t => t.T_THIRD_PARTY_ID, OperationMethod.Equal, thirdPartyId).AndWhere(k => k.T_FROM, OperationMethod.Equal, from);
            var model = this._thirdBll.GetEntity(query);
            if (model != null)
            {
                thirdPartyInfoModel = Mapper.Map<T_THIRD_PARTY, ThirdPartyInfoModel>(model);
            }
            return thirdPartyInfoModel;
        }

        /// <summary>
        /// 获取第三方绑定信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        private ThirdPartyInfoModel getThirdPartyInfoModel(int userID, string from)
        {
            ThirdPartyInfoModel thirdPartyInfoModel = null;
            var query = new DapperExQuery<T_THIRD_PARTY>().AndWhere(t => t.U_USER_ID, OperationMethod.Equal, userID).AndWhere(k => k.T_FROM, OperationMethod.Equal, from);
            var model = this._thirdBll.GetEntity(query);
            if (model != null)
            {
                thirdPartyInfoModel = Mapper.Map<T_THIRD_PARTY, ThirdPartyInfoModel>(model);
            }
            return thirdPartyInfoModel;
        }


        /// <summary>
        /// 校验提交Token
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="mail"></param>
        /// <param name="token"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<bool> checkSubmitToken(EmailAuthCodeType codeType, string mail, string token, string agent)
        {
            bool result = true;
            if (string.IsNullOrEmpty(token))
            {
                Enforce.Throw(new FrontInputValidateErrorException("提交Token不能为空"));
            }
            string redisKey = this.generateSubmitTokenKey(mail, codeType);
            var submitToken = await _database.StringGetAsync(redisKey);
            if (submitToken.Equals(token))
            {
                var tokenContentModel = _tokenSecurity.decrypt<TokenContentModel>(submitToken);
                result = tokenContentModel != null && tokenContentModel.agent.Equals(agent);
            }
            else
            {
                result = false;
            }

            return result;
        }



        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool editPassword(string mail, string password)
        {
            var ret = false;
            var userInfoModel = this.getUserInfoModelByMail(mail, 1);
            if (userInfoModel != null)
            {
                userInfoModel.password = Encrypt.Base64Encode(password);
                var user = Mapper.Map<UserInfoModel, T_USER>(userInfoModel);
                ret = this._userBll.Update(user);
            }
            return ret;
        }


        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<bool> verifyAuthCode(string identity, string code)
        {
            var authCode = await _database.StringGetAsync(identity);
            //不管成功与否，都删除原先旧的Code（异步）
            _database.KeyDeleteAsync(identity);
            if (authCode == code.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 检验重发Token
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="codeType"></param>
        /// <param name="agent"></param>
        /// <param name="reSendToken"></param>
        /// <returns></returns>
        private async Task<bool> verifyReSendAuthMailToken(string mail, EmailAuthCodeType codeType, string agent, string reSendToken)
        {
            string redisKey = this.generateReSendMailTokenKey(mail, codeType);
            var token = await _database.StringGetAsync(redisKey);
            if (reSendToken.Equals(token))
            {
                var tokenContentModel = _tokenSecurity.decrypt<TokenContentModel>(token);
                return tokenContentModel != null
                    && tokenContentModel.agent.Equals(agent)
                    && (DateTime.Now.ToTimeStamp() - tokenContentModel.timestamp) > 10 * 1000;
            }
            else
            {
                return false;
            }
        }



        #endregion



    }

    public class TokenContentModel
    {
        public string agent { set; get; }

        public long timestamp { set; get; }
    }
}
