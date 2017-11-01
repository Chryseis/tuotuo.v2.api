using AutoMapper;
using Mintcode.TuoTuo.v2.BLL;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.Infrastructure.Util;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.IRepository.Models;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.Zeus.Public.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private T_TEAM_BLL _teamBll;
        private T_TEAM_MEMBER_BLL _teamMemberBll;
        private IUserRepository _userRepository;
        private IAttachmentUploadRepository _attachmentUploadRepository;

        private IEmailService _mailService;
        private TemplateHelper _templateHelper;
        private TuoTuoMailConfigurationSection _mailConfig;
        private IFileService _fileService;

        public TeamRepository(T_TEAM_BLL teamBll, T_TEAM_MEMBER_BLL teamMemberBll, IAttachmentUploadRepository attachmentUploadRepository, IUserRepository userRepository, TuoTuoMailConfigurationSection mailConfig, IEmailService mailService, TemplateHelper templateHelper, IFileService fileService)
        {
            _teamBll = teamBll;
            _teamMemberBll = teamMemberBll;
            _userRepository = userRepository;
            _attachmentUploadRepository = attachmentUploadRepository;
             _mailService = mailService;
            _templateHelper = templateHelper;
            _mailConfig = mailConfig;
            _fileService = fileService;
        }

        public List<TeamInfoModel> LoadTeamsByUserID(int userID)
        {
            var inquery = this._teamMemberBll.GetInWhereSql("T_TEAM_ID",
                new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s=>s.U_USER_ID,OperationMethod.Equal, userID));
            var teams=this._teamBll.GetList(new DapperExQuery<T_TEAM>().AndWhere(s=>s.ID,OperationMethod.In, inquery));

            var teamMember = this._teamMemberBll.GetList(new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s => s.U_USER_ID, OperationMethod.Equal, userID));

            List<TeamInfoModel> teminfoList = Mapper.Map<List<T_TEAM>, List<TeamInfoModel>>(teams);
            teminfoList= teminfoList.Select(s => new TeamInfoModel
            {
                createTime = s.createTime,
                createUserMail = s.createUserMail,
                teamAvatar = s.teamAvatar,
                teamID = s.teamID,
                teamName = s.teamName,
                teamSummary = s.teamSummary,
                roleCode = teamMember.SingleOrDefault(m => m.T_TEAM_ID == s.teamID).R_USER_ROLE_CODE
            }).ToList();
            return teminfoList;
        }

        public List<TeamRepoModel> LoadTeamRepoList(int userID,int? state=null)
        {
            var query = new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s => s.U_USER_ID, OperationMethod.Equal, userID);
            if (state.HasValue)
            {
                query.AndWhere(s => s.T_STATE, OperationMethod.Equal, state.Value);
            }
            var teamMembers = this._teamMemberBll.GetList(query);
            var teamMemberList = Mapper.Map<List<T_TEAM_MEMBER>, List<TeamMemberModel>>(teamMembers);

            var teamInfos = this._teamBll.GetList(new DapperExQuery<T_TEAM>().AndWhere(
               s => s.ID,
               OperationMethod.In,
               teamMemberList.Select(s => s.teamID).Distinct().ToArray()));
            var teamInfoList = Mapper.Map<List<T_TEAM>, List<TeamInfoModel>>(teamInfos);

            var allTeamMembers = Mapper.Map < List<T_TEAM_MEMBER>, List< TeamMemberModel >> (
                    this._teamMemberBll.GetList(
                        new DapperExQuery<T_TEAM_MEMBER>()
                        .AndWhere(
                            s=>s.T_TEAM_ID,
                            OperationMethod.In, 
                            teamMemberList.Select(s => s.teamID).Distinct().ToArray()
                            )
                    )
             );


            var teamRepoList = teamInfoList.Select(s => new TeamRepoModel()
            {
                info = s,
                members = allTeamMembers.Where(m => m.teamID.Equals(s.teamID)).ToList()
            }).ToList();
            return teamRepoList;
        }


        public async Task<int> CreateTeam(string teamName, string teamSummary, string avatarToken,
            string currentUserMail)
        {

            var currentUserRepoModel = this._userRepository.GetUser(currentUserMail);
            if (currentUserRepoModel == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户已被删除"));
            }

            string teamAvatar = string.Empty;
            if (!string.IsNullOrEmpty(avatarToken))
            {
                teamAvatar = await _attachmentUploadRepository.GetAttachmentFileID(avatarToken, currentUserRepoModel.info.mail);
            }

            T_TEAM Team = new T_TEAM();
            Team.T_TEAM_CODE = "";
            Team.T_TEAM_NAME = teamName;
            Team.T_TEAM_SUMMARY = teamSummary;
            Team.T_TEAM_AVATAR = teamAvatar;
            Team.T_STATUS = TeamStatus.Enable;
            Team.CREATE_USER_MAIL = currentUserRepoModel.info.mail;
            Team.CREATE_TIME = DateTime.Now;
            return  this._teamBll.CreateTeam(Team, currentUserRepoModel.info.userID, currentUserRepoModel.info.mail, currentUserRepoModel.info.userName);
        }


        public async Task<bool> ModifyTeam(int teamID, string teamName, string teamSummary, string avatarToken, string currentUserMail)
        {
            bool result = false;
            var currentTeam = this._teamBll.GetEntity(new DapperExQuery<T_TEAM>().AndWhere(s => s.ID, OperationMethod.Equal, teamID));
            if (currentTeam!=null)
            {
                currentTeam.T_TEAM_NAME = teamName;
                currentTeam.T_TEAM_SUMMARY = teamSummary;

                string teamAvatar = string.Empty;
                if (!string.IsNullOrEmpty(avatarToken))
                {
                    teamAvatar = await _attachmentUploadRepository.GetAttachmentFileID(avatarToken, currentUserMail);               
       
                }
                if (!string.IsNullOrEmpty(teamAvatar))
                {
                    currentTeam.T_TEAM_AVATAR = teamAvatar;
                }
                result= this._teamBll.Update(currentTeam);
            }
            else
            {
                Enforce.Throw(new LogicErrorException("当前团队已被删除"));
            }

            return result;
        }

        public bool DeleteTeam(int teamID)
        {          
            return this._teamBll.DeleteTeam(teamID);
        }

        public Dictionary<string, int> InviteMembers(int teamID, string currentUserMail, List<string> invitedUserMails)
        {
            invitedUserMails = invitedUserMails.Where(s=>!string.IsNullOrEmpty(s)).Distinct().ToList();

            var dict = new Dictionary<string, int>();
            var currentTeam = this.getTeamInfo(teamID);
            if (currentTeam == null)
            {
                Enforce.Throw(new LogicErrorException("当前团队已被删除"));
            }

            var teamMembers = this.getTeamMemberList(teamID);
            //仅返回存在的用户
            var userList = this._userRepository.GetUserInfoModelList(invitedUserMails);

            //需要插入的未注册的用户
            var insertedUserList = Mapper.Map<List<UserInfoModel>, List<T_USER>>(
                    invitedUserMails
                    .Where(s => !userList.Exists(m => m.mail.Equals(s.ToLower())))
                    .Select(s => new UserInfoModel()
                    {
                        userName = s.Split('@')[0],
                        userTrueName = s.Split('@')[0],
                        userLevel = 1,
                        sex = null,
                        mail = s.ToLower(),
                        //未激活
                        userStatus = 0,
                        lastLoginTime = DateTime.Now,
                        createTime = DateTime.Now
                    })
                    .ToList()
                );
            //需要插入的群成员
            var insertMemberList = invitedUserMails.Where(s => !teamMembers.Exists(m => m.memberMail.Equals(s.ToLower()))).Select(s => new TeamMemberModel()
            {
                teamID = teamID,
                userID = userList.Exists(m => m.mail.Equals(s.ToLower())) ? userList.Single(m => m.mail.Equals(s.ToLower())).userID : 0,
                roleCode = RoleCode.Member,
                tags = new List<string>(),
                state = userList.Exists(m => m.mail.Equals(s.ToLower())) ? 1 : 0,
                memberName = userList.Exists(m => m.mail.Equals(s.ToLower())) ? userList.Single(m => m.mail.Equals(s.ToLower())).userName : string.Empty,
                memberMail = s.ToLower(),
                createUserMail = currentUserMail,
                createTime = DateTime.Now
            }).ToList();

            var insertedTeamMemberList = Mapper.Map<List<TeamMemberModel>, List<T_TEAM_MEMBER>>(insertMemberList);

            this._teamMemberBll.InviteMembers(teamID, insertedUserList, insertedTeamMemberList);

            invitedUserMails.ForEach(s =>
            {
                if (insertMemberList.Exists(m => m.memberMail.Equals(s.ToLower())))
                {
                    dict.Add(s, insertMemberList.Single(m => m.memberMail.Equals(s.ToLower())).state);
                }
            });

            //发送邮件
            foreach (string key in dict.Keys)
            {
                //Todo:后续需要优化
                _mailService.SendMailAsync(key,
                   _mailConfig.InviteContent.Subject.Text,
                    _templateHelper.GenerateContent(_mailConfig.InviteContent.Body.Text, "inviteContent", new { Name = currentTeam.teamName })
                    , true);
            }


            return dict;
           
        }


        public void ExitTeam(int teamID, string currentUserMail)
        {
            var role = this.getUserTeamRole(teamID, currentUserMail);
            if (role.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new LogicErrorException("请先转交团队，再退出团队"));
            }

            this.removeTeamMember(teamID, currentUserMail);

        }



        public bool TransformTeam(int teamID, string oldOwnerMail, string newOwnerMail)
        {
            var member = this.getTeamMember(teamID, newOwnerMail);
            if (member == null)
            {
                Enforce.Throw(new LogicErrorException("转交的用户必须属于当前团队"));
            }

            if (!member.T_STATE.Equals(MemberStatus.Approve))
            {
                Enforce.Throw(new LogicErrorException("转交的用户尚未接收邀请"));
            }
   
            return this._teamMemberBll.TransformTeam(teamID, oldOwnerMail, newOwnerMail.ToLower());
        }

        public void RemoveTeamMember(int teamID, string removedUserMail, string currentUserMail)
        {
            string currentUserRole = this.getUserTeamRole(teamID, currentUserMail);
            string removedUserRole = this.getUserTeamRole(teamID, removedUserMail);
            if (string.IsNullOrEmpty(removedUserRole))
            {
                Enforce.Throw(new LogicErrorException("移除的用户不在当前团队中"));
            }
            if (removedUserRole.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new LogicErrorException("不能移除团队拥有者"));
            }
            if (currentUserRole.Equals(RoleCode.Manager) && removedUserRole.Equals(RoleCode.Manager))
            {
                Enforce.Throw(new UnAuthorizeException("没有权限移除团队管理者"));
            }
            this.removeTeamMember(teamID, removedUserMail);
        }

        public TeamRepoModel GetTeamDetail(int teamID)
        {
            var team = this.getTeamInfo(teamID);
            if (team == null)
            {
                Enforce.Throw(new LogicErrorException("当前项目已被删除"));
            }
            TeamRepoModel teamRepoModel = new TeamRepoModel();
        
            teamRepoModel.info = team;
            teamRepoModel.members = this.getTeamMemberList(teamID);
            return teamRepoModel;
        }


        public bool ChangeMemberRole(int teamID, string changedUserMail, string roleCode)
        {
            bool result = true;
            if (roleCode.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new LogicErrorException("不能设置该角色"));
            }
            var role = this.getUserTeamRole(teamID, changedUserMail);
            if (string.IsNullOrEmpty(role))
            {
                Enforce.Throw(new LogicErrorException("该用户不属于当前团队"));
            }
            if (role.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new UnAuthorizeException("不能设置该用户角色"));
            }
            var member = this.getTeamMember(teamID, changedUserMail);   
            if (member != null)
            {
                member.R_USER_ROLE_CODE = roleCode;
                result = this._teamMemberBll.Update(member);
            }
            else
            {
                result = false;
            }
            return result;

        }

        public bool ChangeMemberTags(int projectID, string currentUserMail, string changedUserMail, List<string> tags)
        {
            bool result = true;
            if (tags == null)
            {
                tags = new List<string>();
            }
            tags = tags.Distinct().ToList();
            var changedMember = this.getTeamMember(projectID, changedUserMail);
            if (changedMember == null)
            {
                Enforce.Throw(new LogicErrorException("该用户不属于当前项目"));
            }
            string currentUserRole = this.getUserTeamRole(projectID, currentUserMail);
            if (currentUserRole.Equals(RoleCode.Manager) && changedMember.R_USER_ROLE_CODE.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new UnAuthorizeException("不能修改该用户标签"));
            }
            changedMember.TAGS = this.generateTagsStringFromArray(tags);
            result = this._teamMemberBll.Update(changedMember);
            return result;

        }

        public List<TeamMemberModel> getTeamMemberList(int teamID)
        {
            //var members = this._teamMemberBll
            //    .GetList(new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID))
            //    .Select(s => new TeamMemberModel
            //    {
            //        ID = s.ID,
            //        teamID = s.T_TEAM_ID,
            //        userID = s.U_USER_ID,
            //        roleCode = s.R_USER_ROLE_CODE,
            //        tags = this.generateTagsArrayFromString(s.TAGS),
            //        state = s.T_STATE,
            //        memberName = s.U_USER_NAME,
            //        memberMail = s.U_USER_EMAIL,
            //        createUserMail = s.CREATE_USER_MAIL,
            //        createTime = s.CREATE_TIME

            //    }).ToList();
            var teamMembers = this._teamMemberBll
                .GetList(new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID)).ToList();
            var userList = this._userRepository.GetUserInfoModelList(teamMembers.Select(s => s.U_USER_EMAIL).ToList());
            var members = teamMembers.Select(s => new TeamMemberModel
            {
                ID = s.ID,
                teamID = s.T_TEAM_ID,
                userID = s.U_USER_ID,
                roleCode = s.R_USER_ROLE_CODE,
                tags = this.generateTagsArrayFromString(s.TAGS),
                state = s.T_STATE,
                memberName = s.U_USER_NAME,
                memberMail = s.U_USER_EMAIL,
                mobile= userList.FirstOrDefault(d=>d.mail==s.U_USER_EMAIL)==null?"":userList.SingleOrDefault(d=>d.mail==s.U_USER_EMAIL).mobile,
                createUserMail = s.CREATE_USER_MAIL,
                createTime = s.CREATE_TIME

            }).ToList();
            return members;
        }

        public SearchTeamAndTeamMemberModel searchTeamAndMember(int userID,string searchName)
        {
            SearchTeamAndTeamMemberModel model = new SearchTeamAndTeamMemberModel();
            var inquery = this._teamMemberBll.GetInWhereSql("T_TEAM_ID",
               new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s => s.U_USER_ID, OperationMethod.Equal, userID));
            var teams = this._teamBll.GetList(new DapperExQuery<T_TEAM>().AndWhere(s => s.ID, OperationMethod.In, inquery));
            var teamMember = this._teamMemberBll.GetList(new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s => s.T_TEAM_ID, OperationMethod.In, inquery));

            List<TeamInfoModel> teminfoList = Mapper.Map<List<T_TEAM>, List<TeamInfoModel>>(teams);
            List<TeamMemberModel> temMemberList = Mapper.Map<List<T_TEAM_MEMBER>, List<TeamMemberModel>>(teamMember);
            model.teamInfos = teminfoList.Where(c => c.teamName.Contains(searchName)).Select(s => new TeamRepoModel
            {
                info = new TeamInfoModel
                {
                    createTime = s.createTime,
                    createUserMail = s.createUserMail,
                    teamAvatar = s.teamAvatar,
                    teamID = s.teamID,
                    teamName = s.teamName,
                    teamSummary = s.teamSummary
                },
                members = temMemberList.Where(w => w.teamID == s.teamID).ToList()
            }).ToList();
            model.teamMembers = temMemberList.Where(c => (c.memberName != null && c.memberName.Contains(searchName)) || c.memberMail.Contains(searchName)).GroupBy(g => new
            {
                g.memberMail
            }).Select(s => new TeamMemberModel()
            {
                memberMail = s.Key.memberMail,
                memberName = temMemberList.LastOrDefault(l=>l.memberMail==s.Key.memberMail).memberName
            }).ToList();
            var userInfo = this._userRepository.GetUser(searchName);
            if (userInfo != null)
            {
                model.teamMembers = model.teamMembers.Where(c => c.memberMail != searchName).ToList();
                TeamMemberModel member = new TeamMemberModel()
                {
                    memberMail = userInfo.info.mail,
                    memberName = userInfo.info.userName
                };
                model.teamMembers.Add(member);
            }
            return model;
        }

        public bool UpdateTeamMemberApproveState(string mail)
        {
            return this._teamMemberBll.UpdateTeamMemberStatus(mail, MemberStatus.Approve);
        }


        public Stream GetTeamAvatar(int teamID, string avatar, int width, int height)
        {
            var teamInfo = this.getTeamInfo(teamID);
            if (teamInfo == null)
            {
                Enforce.Throw(new LogicErrorException("当前项目已被删除"));
            }
            if (string.IsNullOrEmpty(teamInfo.teamAvatar))
            {
                Enforce.Throw(new LogicErrorException("当前项目不存在封面"));
            }
            if (!teamInfo.teamAvatar.Equals(avatar))
            {
                Enforce.Throw(new LogicErrorException("当前项目不存在该封面"));
            }
            byte[] bytes = null;
            string extension = Path.GetExtension(avatar);
            string baseFileNameWithoutExtension = avatar.Remove(avatar.LastIndexOf(extension));
            string fileID = baseFileNameWithoutExtension + "_" + width + "_" + height + extension;
            if (_fileService.CheckFileExist(fileID))
            {
                bytes = _fileService.GetFile(fileID);
            }
            else
            {
                var fileBytes = _fileService.GetFile(avatar);
                bytes = ImageUtils.CompressImage(fileBytes, extension, width, height, "HorW");
                _fileService.UploadFile(fileID, bytes);
            }

            Stream ms = new MemoryStream(bytes);
            return ms;
        }

        private void removeTeamMember(int teamID, string mail)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            this._teamMemberBll.Delete(new DapperExQuery<T_TEAM_MEMBER>()
                .AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID)
                .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, mail)
                );

        }

        /// <summary>
        /// 根据项目ID和用户邮箱获取该用户在项目中的角色
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        private string getUserTeamRole(int teamID, string mail)
        {
            string role = string.Empty;
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            var teamMember = this._teamMemberBll.GetEntity(new DapperExQuery<T_TEAM_MEMBER>()
                .AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID)
                .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, mail));
            if (teamMember != null)
            {
                role = teamMember.R_USER_ROLE_CODE;
            }
            return role;
        }

        private T_TEAM_MEMBER  getTeamMember(int teamID, string mail)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
             return this._teamMemberBll.GetEntity(new DapperExQuery<T_TEAM_MEMBER>()
                .AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID)
                .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, mail)
                );

        }

       
        private List<string> generateTagsArrayFromString(string tags)
        {
            var ret = new List<string>();
            if (!string.IsNullOrEmpty(tags))
            {
                ret = tags.Split(',').ToList();
            }
            return ret;
        }

        private string generateTagsStringFromArray(List<string> tags)
        {
            var ret = string.Empty;
            if (tags != null && tags.Count > 0)
            {
                ret = string.Join(",", tags.ToArray());
            }
            return ret;
        }

        private TeamInfoModel getTeamInfo(int teamID)
        {
            TeamInfoModel infoModel = null;
            var team = this._teamBll.GetEntity(new DapperExQuery<T_TEAM>().AndWhere(s => s.ID, OperationMethod.Equal, teamID));
            if (team != null)
            {
                infoModel = Mapper.Map<T_TEAM, TeamInfoModel>(team);
            }
            return infoModel;
        }


    }
}
