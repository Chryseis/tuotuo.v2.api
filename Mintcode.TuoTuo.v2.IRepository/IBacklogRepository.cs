using Mintcode.TuoTuo.v2.IRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface IBacklogRepository
    {

        /// <summary>
        /// 获取当前BackLog
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="sprintID"></param>
        /// <returns></returns>
        List<BacklogInfoModel> GetBacklogInfoList(int teamID, int sprintID);

        List<BacklogRepoModel> GetBacklogRepoList(int teamID, int sprintID);


        /// <summary>
        /// 设置BackLog
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="sprintID"></param>
        /// <param name="backLogIDs"></param>
        /// <returns></returns>
        bool SetBackLogsSprint(int teamID,int sprintID,List<int> backLogIDs);

        /// <summary>
        /// 创建Backlog
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="standard"></param>
        /// <param name="assignUserMail"></param>
        /// <param name="selectProjectID"></param>
        /// <param name="state"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        BacklogInfoModel CreateBackLog(int teamID,string title,string content,string standard,string assignUserMail,int selectProjectID,int state,int? level, string createUserMail);


        /// <summary>
        /// 更改Backlog
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="backlogID"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="standard"></param>
        /// <param name="assignUserMail"></param>
        /// <param name="selectProjectID"></param>
        /// <param name="state"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        BacklogInfoModel ModifyBackLog(int teamID, int backlogID,string title, string content, string standard, string assignUserMail, int selectProjectID, int state, int? level);


        /// <summary>
        /// 获取Backlog详情
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="backlogID"></param>
        /// <returns></returns>
        BacklogInfoModel GetBacklogInfoModel(int teamID,int backlogID);

        /// <summary>
        /// 删除Backlog
        /// </summary>
        /// <param name="backlogID"></param>
        /// <returns></returns>
        bool DeleteBacklog(int teamID, int backlogID);
    }
}
