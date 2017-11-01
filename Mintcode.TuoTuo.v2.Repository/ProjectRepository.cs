using AutoMapper;
using Mintcode.TuoTuo.v2.BLL;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.Infrastructure.Util;
using Mintcode.TuoTuo.v2.IRepository;
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
    public class ProjectRepository : IProjectRepository
    {

        private T_PROJECT_BLL _projectBll;
        private T_PROJECT_MEMBER_BLL _projectMemberBll;

        private IUserRepository _userRepository;
        private IAttachmentUploadRepository _attachmentUploadRepository;
        private IEmailService _mailService;
        private TemplateHelper _templateHelper;
        private TuoTuoMailConfigurationSection _mailConfig;
        private IFileService _fileService;

        public ProjectRepository(T_PROJECT_BLL projectBll, T_PROJECT_MEMBER_BLL projectMemberBll, TuoTuoMailConfigurationSection mailConfig,
            IAttachmentUploadRepository attachmentUploadRepository, IUserRepository userRepository, IEmailService mailService, TemplateHelper templateHelper,
            IFileService fileService)
        {
            _projectBll = projectBll;
            _projectMemberBll = projectMemberBll;
            _attachmentUploadRepository = attachmentUploadRepository;
            _userRepository = userRepository;
            _mailService = mailService;
            _templateHelper = templateHelper;
            _mailConfig = mailConfig;
            _fileService = fileService;
        }

        public List<ProjectInfoModel> LoadProjectsByUserID(int userID)
        {

            var inquery = this._projectMemberBll.GetInWhereSql("P_PROJECT_ID",
                new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(s => s.U_USER_ID, OperationMethod.Equal, userID));
            var projects = this._projectBll.GetList(new DapperExQuery<T_PROJECT>().AndWhere(s => s.ID, OperationMethod.In, inquery));
            var projectMembers = Mapper.Map<List<T_PROJECT_MEMBER>,List<ProjectMemberModel>>(
                    this._projectMemberBll.GetList(new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(s => s.U_USER_ID, OperationMethod.Equal, userID))
                );
            List<ProjectInfoModel> projectInfoList= Mapper.Map<List<T_PROJECT>, List<ProjectInfoModel>>(projects);
            projectInfoList.ForEach(s =>
            {
                s.roleCode = string.Empty;
                var currentProjectMember = projectMembers.FirstOrDefault(m => m.projectID.Equals(s.projectID));
                if (currentProjectMember != null)
                {
                    s.roleCode = currentProjectMember.roleCode;
                }
                
            });
            return projectInfoList;
        }

        public List<ProjectRepoModel> LoadProjectRepoList(int userID)
        {
            var query = new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(s => s.U_USER_ID, OperationMethod.Equal, userID);
            var projectMembers = this._projectMemberBll.GetList(query);
            var projectMemberList = Mapper.Map<List<T_PROJECT_MEMBER>, List<ProjectMemberModel>>(projectMembers);

            var projectInfos = this._projectBll.GetList(new DapperExQuery<T_PROJECT>().AndWhere(
                s => s.ID,
                OperationMethod.In,
                projectMemberList.Select(s => s.projectID).Distinct().ToArray()));
            var projectInfoList = Mapper.Map<List<T_PROJECT>, List<ProjectInfoModel>>(projectInfos);

            var projectRepoList = projectInfoList.Select(s => new ProjectRepoModel()
            {
                info = s,
                members = projectMemberList.Where(m => m.projectID.Equals(s.projectID)).ToList()
            }).ToList();
            return projectRepoList;
        }

        public IDictionary<int, List<ProjectInfoModel>> LoadProjectInfosByUserArray(List<int>userList)
        {
            IDictionary<int, List<ProjectInfoModel>> dict = new Dictionary<int, List<ProjectInfoModel>>();
            if (userList!=null && userList.Count>0)
            {
                userList = userList.Distinct().ToList();
                var query = new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(s => s.U_USER_ID, OperationMethod.In, userList.ToArray());
                var projectMembers = this._projectMemberBll.GetList(query);
                var projectMemberList = Mapper.Map<List<T_PROJECT_MEMBER>, List<ProjectMemberModel>>(projectMembers);

                var projectInfos = this._projectBll.GetList(new DapperExQuery<T_PROJECT>().AndWhere(
                    s => s.ID,
                    OperationMethod.In,
                    projectMemberList.Select(s => s.projectID).Distinct().ToArray()));
                var projectInfoList = Mapper.Map<List<T_PROJECT>, List<ProjectInfoModel>>(projectInfos);

                dict= userList.ToDictionary(
                    s=>s,
                    s=> projectInfoList.Where(
                        n=> projectMemberList.Where(m => m.userID.Equals(s)).Select(m => m.projectID).ToList().Contains(n.projectID)).ToList());
            }
           
            return dict;

        } 

        public List<ProjectInfoModel> LoadProjectsByUserMail(string userMail)
        {
            var userInfoModel=this._userRepository.GetUser(userMail,1);
            if (userInfoModel==null)
            {
                Enforce.Throw(new LogicErrorException("用户不存在"));
            }
            return this.LoadProjectsByUserID(userInfoModel.info.userID);
        }


        public async Task<ProjectInfoModel> CreateProject(string projectName, string projectSummary, string avatarToken,string currentUserMail)
        {
            var currentUserRepoModel = this._userRepository.GetUser(currentUserMail);
            if (currentUserRepoModel==null)
            {
                Enforce.Throw(new LogicErrorException("当前用户已被删除"));
            }
                     
            string projectAvatar = string.Empty;
            if (!string.IsNullOrEmpty(avatarToken))
            {
                projectAvatar = await _attachmentUploadRepository.GetAttachmentFileID(avatarToken, currentUserRepoModel.info.mail);
            }
            ProjectInfoModel infoModel = new ProjectInfoModel();
            infoModel.projectName = projectName;
            infoModel.projectSummary = projectSummary;
            infoModel.projectAvatar = projectAvatar;
            infoModel.createUserMail= currentUserRepoModel.info.mail;
            infoModel.createTime= DateTime.Now;

            var project=Mapper.Map<ProjectInfoModel, T_PROJECT>(infoModel);
            if (this._projectBll.CreateProject(project, currentUserRepoModel.info.userID, currentUserRepoModel.info.userName))
            {
                infoModel = Mapper.Map<T_PROJECT, ProjectInfoModel>(project);
            }
            return infoModel;

        }


        public async Task<bool> ModifyProject(int projectID, string projectName, string projectSummary, string avatarToken, string currentUserMail)
        {
            bool result = false; 
            var currentProject = this.getProjectInfo(projectID); ;
            if (currentProject==null)
            {
                Enforce.Throw(new LogicErrorException("当前项目已被删除"));
            }
            currentProject.projectName = projectName;
            currentProject.projectSummary = projectSummary;
            string projectAvatar = string.Empty;
            if (!string.IsNullOrEmpty(avatarToken))
            {
                projectAvatar = await _attachmentUploadRepository.GetAttachmentFileID(avatarToken, currentUserMail);               
            }
            if (!string.IsNullOrEmpty(projectAvatar))
            {
                currentProject.projectAvatar = projectAvatar;
            }
            var updatedProject = Mapper.Map<ProjectInfoModel, T_PROJECT>(currentProject);
            result = this._projectBll.Update(updatedProject);
            return result;
        }

        public bool DeleteProject(int projectID)
        {          
            return this._projectBll.DeleteProject(projectID);
        }

        public Dictionary<string, int> InviteMembers(int projectID,string currentUserMail,List<string> invitedUserMails)
        {
            invitedUserMails=invitedUserMails.Where(s=>!string.IsNullOrEmpty(s)).Distinct().ToList();

            var dict = new Dictionary<string, int>();     
            var currentProject = this.getProjectInfo(projectID);
            if (currentProject == null)
            {
                Enforce.Throw(new LogicErrorException("当前项目已被删除"));
            }
            var projectMembers = this.getProjectMemberList(projectID);
            var userList=this._userRepository.GetUserInfoModelList(invitedUserMails);
           
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
                        userStatus = 0,
                        lastLoginTime = DateTime.Now,
                        createTime = DateTime.Now
                    })
                    .ToList()
                );

            var projectMemberList = invitedUserMails.Where(s => !projectMembers.Exists(m => m.memberMail.Equals(s.ToLower()))).Select(s => new ProjectMemberModel()
            {
                projectID = projectID,
                userID = userList.Exists(m => m.mail.Equals(s.ToLower())) ? userList.Single(m => m.mail.Equals(s.ToLower())).userID : 0,
                roleCode = RoleCode.Member,
                tags = new List<string>(),
                state = userList.Exists(m => m.mail.Equals(s.ToLower())) ? 1 : 0,
                memberName = userList.Exists(m => m.mail.Equals(s.ToLower())) ? userList.Single(m => m.mail.Equals(s.ToLower())).userName : string.Empty,
                memberMail = s.ToLower(),
                createUserMail = currentUserMail,
                createTime = DateTime.Now
            }).ToList();
            invitedUserMails.ForEach(s =>
            {
                if (projectMemberList.Exists(m => m.memberMail.Equals(s.ToLower())))
                {
                    dict.Add(s, projectMemberList.Single(m => m.memberMail.Equals(s.ToLower())).state);
                }
            });
           
            var insertedProjectMemberList = Mapper.Map<List<ProjectMemberModel>, List<T_PROJECT_MEMBER>>(projectMemberList);

            this._projectMemberBll.InviteMembers(projectID,insertedUserList, insertedProjectMemberList);


            //发送邮件
            foreach (string key in dict.Keys)
            {
                //Todo:后续需要优化
                _mailService.SendMailAsync(key,
                   _mailConfig.InviteContent.Subject.Text,
                    _templateHelper.GenerateContent(_mailConfig.InviteContent.Body.Text, "inviteContent", new { Name = currentProject.projectName })
                    , true);
            }

            return dict;
        }


        public void ExitProject(int projectID,string currentUserMail)
        {         
            var role = this.getUserProjectRole(projectID, currentUserMail);
            if (role.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new LogicErrorException("请先转交项目，再退出项目"));
            }
                       
            this.deleteProjectMember(projectID, currentUserMail);

        }

     
        public bool TransformProject(int projectID, string oldOwnerMail, string newOwnerMail)
        {
            var member = this.GetProjectMember(projectID, newOwnerMail);
            if (member == null)
            {
                Enforce.Throw(new LogicErrorException("转交的用户必须属于当前项目"));
            }

            if (!member.state.Equals(1))
            {
                Enforce.Throw(new LogicErrorException("转交的用户尚未接收邀请"));
            }
                     
            return this._projectMemberBll.TransformProject(projectID, oldOwnerMail, newOwnerMail.ToLower());
        }


        public void RemoveProjectMember(int projectID, string removedUserMail, string currentUserMail)
        {
            string currentUserRole = this.getUserProjectRole(projectID, currentUserMail);
            string removedUserRole = this.getUserProjectRole(projectID, removedUserMail);
            if (string.IsNullOrEmpty(removedUserRole))
            {
                Enforce.Throw(new LogicErrorException("移除的用户不在当前项目中"));    
            }
            if (removedUserRole.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new LogicErrorException("不能移除项目拥有者"));
            }
            if (currentUserRole.Equals(RoleCode.Manager) && removedUserRole.Equals(RoleCode.Manager))
            {
                Enforce.Throw(new UnAuthorizeException("没有权限移除项目管理者"));
            }
            this.deleteProjectMember(projectID, removedUserMail);
        }


        public ProjectRepoModel GetProjectDetail(int projectID)
        {           
            var project= this.getProjectInfo(projectID);
            if (project==null)
            {
                Enforce.Throw(new LogicErrorException("当前项目已被删除"));
            }
            ProjectRepoModel projectRepoModel = new ProjectRepoModel();
            projectRepoModel.info = project;          
            projectRepoModel.members = this.getProjectMemberList(projectID);
            return projectRepoModel;
        }


        public bool ChangeMemberRole(int projectID, string changedUserMail, string roleCode)
        {
            bool result = true;
            if (roleCode.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new LogicErrorException("不能设置该角色"));
            }
            var changedMember = this.GetProjectMember(projectID, changedUserMail);
            if (changedMember==null)
            {
                Enforce.Throw(new LogicErrorException("该用户不属于当前项目"));    
            }
            if (changedMember.roleCode.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new UnAuthorizeException("不能设置该用户角色"));     
            }
            changedMember.roleCode = roleCode;

            var projectMember = Mapper.Map<ProjectMemberModel, T_PROJECT_MEMBER>(changedMember);
            result = this._projectMemberBll.Update(projectMember);

            return result;

        }


        public bool ChangeMemberTags(int projectID,string currentUserMail, string changedUserMail, List<string> tags)
        {
            bool result = true;
            if (tags==null)
            {
                tags = new List<string>();
            }
            tags = tags.Distinct().ToList();
            var changedMember = this.GetProjectMember(projectID, changedUserMail);
            if (changedMember == null)
            {
                Enforce.Throw(new LogicErrorException("该用户不属于当前项目"));
            }
            string currentUserRole = this.getUserProjectRole(projectID, currentUserMail);
            if (currentUserRole.Equals(RoleCode.Manager) && changedMember.roleCode.Equals(RoleCode.Owner))
            {
                Enforce.Throw(new UnAuthorizeException("不能修改该用户标签"));
            }
            changedMember.tags = tags;

            var projectMember=Mapper.Map<ProjectMemberModel,T_PROJECT_MEMBER>(changedMember);   
                 
            result = this._projectMemberBll.Update(projectMember);

            return result;

        }


        /// <summary>
        /// 根据项目ID和用户邮箱获取该用户在该项目中的信息
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        public ProjectMemberModel GetProjectMember(int projectID, string mail)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            ProjectMemberModel model = null;
            var member = this._projectMemberBll.GetEntity(new DapperExQuery<T_PROJECT_MEMBER>()
                .AndWhere(s => s.P_PROJECT_ID, OperationMethod.Equal, projectID)
                .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, mail)
                );
            if (member != null)
            {
                model = Mapper.Map<T_PROJECT_MEMBER, ProjectMemberModel>(member);
            }
            return model;


        }

        public bool UpdateProjectMemberApproveState(string mail)
        {
            return this._projectMemberBll.UpdateProjectMemberStatus(mail, MemberStatus.Approve);
        }


        public Stream GetProjectAvatar(int projectID, string avatar, int width, int height)
        {
            var projectInfo=this.getProjectInfo(projectID);
            if (projectInfo == null)
            {
                Enforce.Throw(new LogicErrorException("当前项目已被删除"));
            }
            if (string.IsNullOrEmpty(projectInfo.projectAvatar))
            {
                Enforce.Throw(new LogicErrorException("当前项目不存在封面"));
            }
            if (!projectInfo.projectAvatar.Equals(avatar))
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


        #region 私有方法

        /// <summary>
        /// 删除项目成员
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="mail"></param>
        private void deleteProjectMember(int projectID, string mail)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            this._projectMemberBll.Delete(new DapperExQuery<T_PROJECT_MEMBER>()
                .AndWhere(s => s.P_PROJECT_ID, OperationMethod.Equal, projectID)
                .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, mail)
                );

        }


        /// <summary>
        /// 根据项目ID和用户邮箱获取该用户在项目中的角色
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        private string getUserProjectRole(int projectID, string mail)
        {
            mail = string.IsNullOrEmpty(mail) ? mail : mail.ToLower();
            string role = string.Empty;
            var projectMember = this._projectMemberBll.GetEntity(new DapperExQuery<T_PROJECT_MEMBER>()
                .AndWhere(s => s.P_PROJECT_ID, OperationMethod.Equal, projectID)
                .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, mail));
            if (projectMember != null)
            {
                role = projectMember.R_ROLE_CODE;
            }
            return role;
        }


        

        /// <summary>
        /// 根据项目ID获取项目成员列表
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private List<ProjectMemberModel> getProjectMemberList(int projectID)
        {
            
            //var members = this._projectMemberBll
            //    .GetList(new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(s => s.P_PROJECT_ID, OperationMethod.Equal, projectID))
            //    .Select(s => Mapper.Map<T_PROJECT_MEMBER, ProjectMemberModel>(s))
            //    .ToList();

            var projectMembers = this._projectMemberBll
                   .GetList(new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(s => s.P_PROJECT_ID, OperationMethod.Equal, projectID))
                .Select(s => Mapper.Map<T_PROJECT_MEMBER, ProjectMemberModel>(s))
                .ToList();
            var userList = this._userRepository.GetUserInfoModelList(projectMembers.Select(s => s.memberMail).ToList());
            var members = projectMembers.Select(s => new ProjectMemberModel
            {
                ID = s.ID,
                projectID = s.projectID,
                userID = s.userID,
                roleCode = s.roleCode,
                tags = s.tags,
                state = s.state,
                memberName = s.memberName,
                memberMail = s.memberMail,
                mobile = userList.FirstOrDefault(d => d.mail == s.memberMail) == null ? "" : userList.SingleOrDefault(d => d.mail == s.memberMail).mobile,
                createUserMail = s.createUserMail,
                createTime = s.createTime

            }).ToList();
            return members;
            return members;
        }


        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private ProjectInfoModel getProjectInfo(int projectID)
        {
            ProjectInfoModel infoModel = null;
            var project=this._projectBll.GetEntity(new DapperExQuery<T_PROJECT>().AndWhere(s => s.ID, OperationMethod.Equal, projectID));
            if (project!=null)
            {
                infoModel=Mapper.Map<T_PROJECT, ProjectInfoModel>(project);
            }
            return infoModel;
        }


        #endregion

    }
}
