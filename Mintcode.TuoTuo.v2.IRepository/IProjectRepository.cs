using Mintcode.TuoTuo.v2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface IProjectRepository
    {

        /// <summary>
        /// 根据用户ID获取拥有或者参与的项目
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        List<ProjectInfoModel> LoadProjectsByUserID(int userID);


        /// <summary>
        /// 根据用户ID获取用户拥有或者参与的项目的详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<ProjectRepoModel> LoadProjectRepoList(int userID);

        /// <summary>
        /// 获取拥有或者参与的项目的列表
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        IDictionary<int, List<ProjectInfoModel>> LoadProjectInfosByUserArray(List<int> userList);


        /// <summary>
        /// 根据用户邮箱获取拥有或者参与的项目
        /// </summary>
        /// <param name="userMail"></param>
        /// <returns></returns>
        List<ProjectInfoModel> LoadProjectsByUserMail(string userMail);

        /// <summary>
        /// 创建项目
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectSummary">项目描述</param>
        /// <param name="avatarToken">项目封面文件的Token</param>
        /// <param name="currentUserMail">当前用户邮箱</param>
        /// <returns></returns>
        Task<ProjectInfoModel> CreateProject(string projectName,string projectSummary,string avatarToken,string currentUserMail);


        /// <summary>
        /// 更改项目信息
        /// </summary>
        /// <param name="projectID">项目ID</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectSummary">项目描述</param>
        /// <param name="avatarToken">项目封面文件的Token</param>
        /// <returns></returns>
        Task<bool> ModifyProject(int projectID, string projectName, string projectSummary, string avatarToken, string currentUserMail);

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        bool DeleteProject(int projectID);


        /// <summary>
        /// 邀请成员
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="currentUserMail"></param>
        /// <param name="invitedUserMails"></param>
        /// <returns></returns>
        Dictionary<string, int> InviteMembers(int projectID, string currentUserMail, List<string> invitedUserMails);


        /// <summary>
        /// 退出项目
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="currentUserMail"></param>
        /// <returns></returns>
        void ExitProject(int projectID, string currentUserMail);


        /// <summary>
        /// 转交项目
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="createMail"></param>
        /// <param name="userID"></param>
        /// <param name="mail"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool TransformProject(int projectID, string oldOwnerMail, string newOwnerMail);


        /// <summary>
        /// 移除项目成员
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="removedUserMail"></param>
        /// <param name="currentMail"></param>
        void RemoveProjectMember(int projectID,string removedUserMail,string currentUserMail);

      

        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        ProjectRepoModel GetProjectDetail(int projectID);

        /// <summary>
        /// 修改成员角色
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="changedUserMail"></param>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        bool ChangeMemberRole(int projectID,string changedUserMail, string roleCode);


        /// <summary>
        /// 修改成员标签
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="currentUserMail"></param>
        /// <param name="changedUserMail"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        bool ChangeMemberTags(int projectID, string currentUserMail, string changedUserMail, List<string> tags);


        /// <summary>
        /// 根据项目ID和用户邮箱获取该用户在该项目中的信息
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        ProjectMemberModel GetProjectMember(int projectID, string mail);

        /// <summary>
        /// 更新项目成员状态（同意邀请）
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        bool UpdateProjectMemberApproveState(string mail);


        /// <summary>
        /// 获取项目封面
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        Stream GetProjectAvatar(int projectID,string avatar,int width,int height);
    }
}
