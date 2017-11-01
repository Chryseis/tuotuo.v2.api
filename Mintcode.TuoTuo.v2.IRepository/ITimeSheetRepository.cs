using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.IRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface ITimeSheetRepository
    {

        /// <summary>
        /// 根据邮箱和时间戳获取TimeSheet详情
        /// </summary>
        /// <param name="currentUserMail"></param>
        /// <param name="currentTimeStamp"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        TimeSheetRepoModel GetTimeSheetDetail(string currentUserMail, long currentTimeStamp, bool throwException = true);

        /// <summary>
        /// 获取审批的TimeSheet数据列表
        /// </summary>
        /// <param name="currentUserMail"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="selectTeamIDList"></param>
        /// <param name="selectUserIDList"></param>
        /// <param name="selectStatus"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        List<TimeSheetRepoModel> GetCheckTimeSheetRepoModelList(int userID, long startTimeStamp, long endTimeStamp,
            List<int> selectTeamIDList, List<int> selectUserIDList, List<int> selectStatusList,
            int from,int to,out long total);

        /// <summary>
        /// 获取TimeSheet 报表列表
        /// </summary>
        /// <param name="currentUserMail"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="selectTeamIDList"></param>
        /// <param name="selectUserIDList"></param>
        /// <param name="selectProjectIDList"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="total"></param>
        TimeSheetReportModel GetReportTimeSheetList(int userID, long startTimeStamp, long endTimeStamp,
            List<int> selectTeamIDList, List<int> selectUserIDList, List<int> selectProjectIDList,
            int from, int to, out long total);

        /// <summary>
        /// 创建Time Sheet
        /// </summary>
        /// <param name="currentUserMail"></param>
        /// <param name="currentTimeStamp"></param>
        /// <returns></returns>
        bool CreateTimeSheet(string currentUserMail, long currentTimeStamp);

        /// <summary>
        /// 创建Time Sheet Tasks
        /// </summary>
        /// <param name="sheetID"></param>
        /// <param name="tasks"></param>
        /// <param name="currentUserMail"></param>
        /// <returns></returns>
        List<TimeSheetTaskModel> CreateTimeSheetTasks(int sheetID, List<CreateTimeSheetTaskModel> tasks, string currentUserMail);

        /// <summary>
        /// 删除TimeSheet Task
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="currentUserMail"></param>
        void DeleteTimeSheetTask(int taskID,string currentUserMail);

        /// <summary>
        /// 更改TimeSheet Task
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="detail"></param>
        /// <param name="selectProjectID"></param>
        /// <param name="time"></param>
        /// <param name="currentUserMail"></param>
        /// <returns></returns>
        bool ModifyTimeSheetTask(int taskID,string detail,int selectProjectID,decimal time,string currentUserMail);

        /// <summary>
        /// 审批TimeSheet
        /// </summary>
        /// <param name="sheetID"></param>
        /// <param name="result"></param>
        /// <param name="comment"></param>
        /// <param name="viewTimeStamp"></param>
        /// <param name="currentUserMail"></param>
        /// <returns></returns>
        bool ApproveTimeSheet(int sheetID, TimeSheetResultStatus result, string comment, long viewTimeStamp, string currentUserMail);

        /// <summary>
        /// 提交TimeSheet
        /// </summary>
        /// <param name="sheetID"></param>
        /// <param name="currentUserMail"></param>
        /// <returns></returns>
        bool SubmitTimeSheet(int sheetID, string currentUserMail);


        /// <summary>
        /// TimeSheet查询参数
        /// </summary>
        /// <param name="currentUserMail"></param>
        /// <returns></returns>
        List<TimeSheetQueryModel> GetQueryParams(int userID);

    }
}
