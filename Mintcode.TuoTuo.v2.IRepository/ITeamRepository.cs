using Mintcode.TuoTuo.v2.IRepository.Models;
using Mintcode.TuoTuo.v2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
   public  interface ITeamRepository
    {
        /// <summary>
        /// 根据用户ID获取拥有或者参与的团队
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
       List<TeamInfoModel> LoadTeamsByUserID(int userID);


        /// <summary>
        ///  根据用户ID获取拥有或者参与的团队的详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<TeamRepoModel> LoadTeamRepoList(int userID, int? state=null);

        /// <summary>
        /// 创建团队
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="teamSummary"></param>
        /// <param name="avatarToken"></param>
        /// <param name="currentUserMail"></param>
        /// <returns></returns>
        Task<int> CreateTeam(string teamName, string teamSummary, string avatarToken,
            string currentUserMail);

        /// <summary>
        /// 更新团队
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="teamName"></param>
        /// <param name="teamSummary"></param>
        /// <param name="avatarToken"></param>
        /// <param name="currentUserMail"></param>
        /// <returns></returns>
        Task<bool> ModifyTeam(int teamID, string teamName, string teamSummary, string avatarToken, string currentUserMail);

        /// <summary>
        /// 删除团队
        /// </summary>
        /// <param name="TeamID"></param>
        /// <returns></returns>
        bool DeleteTeam(int TeamID);

        /// <summary>
        /// 邀请成员
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="currentUserMail"></param>
        /// <param name="invitedUserMails"></param>
        /// <returns></returns>
        Dictionary<string, int> InviteMembers(int teamID, string currentUserMail, List<string> invitedUserMails);

       /// <summary>
       /// 退出团队
       /// </summary>
       /// <param name="teamID"></param>
       /// <param name="currentUserMail"></param>
        void ExitTeam(int teamID, string currentUserMail);

        /// <summary>
        /// 转交团队
        /// </summary>
        /// <param name="TeamID"></param>
        /// <param name="createMail"></param>
        /// <param name="userID"></param>
        /// <param name="mail"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool TransformTeam(int TeamID, string oldOwnerMail, string newOwnerMail);


       /// <summary>
       /// 移除团队成员
       /// </summary>
       /// <param name="teamID"></param>
       /// <param name="removedUserMail"></param>
       /// <param name="currentUserMail"></param>
       void RemoveTeamMember(int teamID, string removedUserMail, string currentUserMail);

       /// <summary>
       /// 获取团队详情
       /// </summary>
       /// <param name="teamID"></param>
       /// <returns></returns>
       TeamRepoModel GetTeamDetail(int teamID);

       /// <summary>
       /// 修改成员角色
       /// </summary>
       /// <param name="teamID"></param>
       /// <param name="changedUserMail"></param>
       /// <param name="roleCode"></param>
       /// <returns></returns>
       bool ChangeMemberRole(int teamID, string changedUserMail, string roleCode);

       /// <summary>
       /// 修改团队成员标签
       /// </summary>
       /// <param name="projectID"></param>
       /// <param name="currentUserMail"></param>
       /// <param name="changedUserMail"></param>
       /// <param name="tags"></param>
       /// <returns></returns>
       bool ChangeMemberTags(int projectID, string currentUserMail, string changedUserMail, List<string> tags);

       /// <summary>
       /// 根据teamID 获取团队成员
       /// </summary>
       /// <param name="teamID"></param>
       /// <returns></returns>
       List<TeamMemberModel> getTeamMemberList(int teamID);

       /// <summary>
       /// 搜索人员
       /// </summary>
       /// <param name="userID"></param>
       /// <param name="searchName"></param>
       /// <returns></returns>
       SearchTeamAndTeamMemberModel searchTeamAndMember(int userID, string searchName);

       /// <summary>
       /// 更新团队成员状态（同意邀请）
       /// </summary>
       /// <param name="mail"></param>
       /// <returns></returns>
       bool UpdateTeamMemberApproveState(string mail);

       /// <summary>
       /// 获取团队头像
       /// </summary>
       /// <param name="teamID"></param>
       /// <param name="avatar"></param>
       /// <returns></returns>
       Stream GetTeamAvatar(int teamID, string avatar, int width, int height);
    }
}
