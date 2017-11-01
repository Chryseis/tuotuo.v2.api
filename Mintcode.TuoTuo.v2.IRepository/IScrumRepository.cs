using Mintcode.TuoTuo.v2.IRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface IScrumRepository
    {
        /// <summary>
        /// 创建Release
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="releaseName"></param>
        /// <param name="releaseSummary"></param>
        /// <returns></returns>
        ReleaseInfoModel CreateRelease(int teamID,string releaseName,string releaseSummary,string currentMail);

        /// <summary>
        /// Release 详情
        /// </summary>
        /// <param name="releaseID"></param>
        /// <returns></returns>
        ReleaseInfoModel ViewRelease(int teamID,int releaseID);

        /// <summary>
        /// Release 列表
        /// </summary>
        /// <param name="teamID"></param>
        /// <returns></returns>
        List<ReleaseInfoModel> ListRelease(int teamID);


        /// <summary>
        /// 删除Release
        /// </summary>
        /// <param name="teamID"></param>
        /// <returns></returns>
        bool DeleteRelease(int teamID, int releaseID);


        /// <summary>
        /// 创建Sprint
        /// </summary>
        /// <param name="releaseID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="currentMail"></param>
        /// <returns></returns>
        SprintInfoModel CreateSprint(int teamID,int releaseID, DateTime startTime, DateTime endTime, string currentMail);


        /// <summary>
        /// Sprint列表
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="releaseID"></param>
        /// <returns></returns>
        List<SprintInfoModel> ListSprint(int teamID,int releaseID);


        /// <summary>
        /// 设置当前Sprint
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="sprintID"></param>
        /// <returns></returns>
        bool SetCurrentSprint(int teamID,int sprintID);


        /// <summary>
        /// 编辑Sprint
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="sprintID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        bool EditSprint(int teamID, int sprintID, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 删除Sprint
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="sprintID"></param>
        /// <returns></returns>
        void DeleteSprint(int teamID, int sprintID);


        /// <summary>
        /// 根据团队ID和Sprint ID获取Sprint
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="sprintID"></param>
        /// <returns></returns>
        SprintInfoModel GetSprintInfoByTeamIDAndSprintID(int teamID, int sprintID);


        /// <summary>
        /// 根据团队ID获取Release 和 Sprint 列表
        /// </summary>
        /// <param name="teamID"></param>
        /// <returns></returns>
        List<ReleaseRepoModel> GetReleaseAndSprintList(int teamID);

        /// <summary>
        /// 获取团队当前的Release 和Sprint
        /// </summary>
        /// <param name="teamID"></param>
        /// <returns></returns>
        CurrentReleaseAndSprint GetCurrentReleaseAndSprint(int teamID);

        void SetBacklogRepository(IBacklogRepository backlogRepository);

    }
}
