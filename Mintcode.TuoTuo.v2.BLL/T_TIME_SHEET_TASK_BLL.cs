using Dapper;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.Zeus.Public.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.BLL
{
    public partial class T_TIME_SHEET_TASK_BLL
    {
        //Todo:暂时性这么解决
        public bool AddTaskList(List<T_TIME_SHEET_TASK> tasks, T_TIME_SHEET sheet )
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.Update(sheet);
                    foreach (var task in tasks)
                    {
                        DbContext.Insert(task);
                    }
                }
                catch (Exception e)
                {
                    isSucess = false;
                    throw e;
                }
                finally
                {
                    if (isSucess)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                return isSucess;
            }
        }


        public bool ModifyTask(T_TIME_SHEET_TASK task, T_TIME_SHEET sheet)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.Update(sheet);
                    DbContext.Update(task);                   
                }
                catch (Exception e)
                {
                    isSucess = false;
                    throw e;
                }
                finally
                {
                    if (isSucess)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                return isSucess;
            }
        }


        public List<T_USER> GetReportPageList(List<int> selectTeamIDList,List<int> selectUserIDList, List<int> selectProjectIDList,int from,int to,out long total)
        {      
            var db = DbContext.DbConnecttion;
            total = 0;
            int pageSize = to - from + 1;
            int pageIndex = (int)((double)from / pageSize) + 1;
            var param = new
            {
                TeamIDs = selectTeamIDList,
                UserIDs = selectUserIDList,
                ProjectIDs = selectProjectIDList,
                First= (pageIndex - 1)* pageSize,
                Size= pageSize
            };
            StringBuilder condationSqlBuilder = new StringBuilder();
            if (selectTeamIDList!=null && selectTeamIDList.Count>0)
            {
                condationSqlBuilder.Append("and t_team_member.T_TEAM_ID in @TeamIDs ");
            }
            if (selectUserIDList != null && selectUserIDList.Count > 0)
            {
                condationSqlBuilder.Append("and t_team_member.U_USER_ID in @UserIDs ");
            }
            if (selectProjectIDList != null && selectProjectIDList.Count > 0)
            {
                condationSqlBuilder.Append("and t_project_member.P_PROJECT_ID in @ProjectIDs ");
            }
            string condationSql = condationSqlBuilder.ToString();

            var querySql = @"select 
                                t_team_member.U_USER_ID as ID,
                                t_team_member.U_USER_EMAIL as U_USER_NAME,
                                t_team_member.U_USER_NAME as U_EMAIL
                                from t_team_member
                                left join t_project_member on t_team_member.U_USER_ID = t_project_member.U_USER_ID
                                where t_team_member.T_STATE=1 " + condationSql + "group by t_team_member.U_USER_ID,t_team_member.U_USER_EMAIL,t_team_member.U_USER_NAME limit @First,@Size";

            var countSql= @"select count(*) DataCount 
                                    from(
                                        select 
                                        t_team_member.U_USER_ID as ID
                                        from t_team_member
                                        left join t_project_member on t_team_member.U_USER_ID = t_project_member.U_USER_ID
                                        where t_team_member.T_STATE=1  " + condationSql + "group by t_team_member.U_USER_ID,t_team_member.U_USER_EMAIL,t_team_member.U_USER_NAME) Report";

            var cr = db.Query(countSql, param, null, false, null, null).SingleOrDefault();
            total = cr==null?0:(long)cr.DataCount;
            var userList = db.Query<T_USER>(querySql, param, null, false, null, null).ToList();
                   
            return userList;

        }

        public decimal GetReportTotalTime(long startTimeStamp,long endTimeStamp,List<int> selectTeamIDList, List<int> selectUserIDList, List<int> selectProjectIDList)
        {
            var db = DbContext.DbConnecttion;
            var param = new
            {
                TeamIDs = selectTeamIDList,
                UserIDs = selectUserIDList,
                ProjectIDs = selectProjectIDList  
            };
            StringBuilder condationSqlBuilder = new StringBuilder();
            if (selectTeamIDList != null && selectTeamIDList.Count > 0)
            {
                condationSqlBuilder.Append("and t_team_member.T_TEAM_ID in @TeamIDs ");
            }
            if (selectUserIDList != null && selectUserIDList.Count > 0)
            {
                condationSqlBuilder.Append("and t_team_member.U_USER_ID in @UserIDs ");
            }
            if (selectProjectIDList != null && selectProjectIDList.Count > 0)
            {
                condationSqlBuilder.Append("and t_project_member.P_PROJECT_ID in @ProjectIDs ");
            }
            string condationSql = condationSqlBuilder.ToString();
            var querySql = @"select 
                                t_team_member.U_USER_ID
                                from t_team_member
                                left join t_project_member on t_team_member.U_USER_ID = t_project_member.U_USER_ID
                                where t_team_member.T_STATE=1 " + condationSql + "group by t_team_member.U_USER_ID,t_team_member.U_USER_EMAIL,t_team_member.U_USER_NAME";

            var userIDList = db.Query<int>(querySql, param, null, false, null, null).ToList();
            if (userIDList.Count<=0)
            {
                return 0;
            }
            var totalTimeQuerySql = @"select sum(t_time_sheet_task.TST_TIME) TotalTime
                                        from t_time_sheet_task 
                                        INNER JOIN t_time_sheet on t_time_sheet_task.TS_ID=t_time_sheet.ID
                                        where t_time_sheet.TS_STATUS=@status
                                        and t_time_sheet.TS_TIMESTAMP>=@startTimeStamp
                                        and t_time_sheet.TS_TIMESTAMP<=@endTimeStamp
                                        and t_time_sheet_task.TST_USER_ID in @UserIDs";
            var totalTimeList = db.Query(totalTimeQuerySql, new {
                status=(int)TimeSheetStatus.Checked,
                startTimeStamp = startTimeStamp ,
                endTimeStamp =endTimeStamp,
                UserIDs = userIDList }, null, false, null, null).SingleOrDefault();
            var totalTime= totalTimeList==null || totalTimeList.TotalTime==null? 0: (decimal)totalTimeList.TotalTime;
            return totalTime;

        }

        public List<T_TIME_SHEET_TASK> GetReportUserProjectTime(long startTimeStamp, long endTimeStamp, List<int> userIDList, List<int> selectProjectIDList)
        {
            var db = DbContext.DbConnecttion;
            var param = new
            {
                status = (int)TimeSheetStatus.Checked,
                startTimeStamp = startTimeStamp,
                endTimeStamp = endTimeStamp,  
                UserIDs = userIDList,
                ProjectIDs = selectProjectIDList
            };
            StringBuilder condationSqlBuilder = new StringBuilder();
            if (userIDList != null && userIDList.Count > 0)
            {
                condationSqlBuilder.Append("and t_time_sheet_task.TST_USER_ID in @UserIDs ");
            }
            if (selectProjectIDList != null && selectProjectIDList.Count > 0)
            {
                condationSqlBuilder.Append("and t_time_sheet_task.P_ID in @ProjectIDs ");
            }

            var querySql = @"select 
                                t_time_sheet_task.TST_USER_ID,
                                t_time_sheet_task.P_ID,
                                t_time_sheet_task.P_NAME,
                                sum(t_time_sheet_task.TST_TIME) TST_TIME
                                from t_time_sheet_task 
                                inner join t_time_sheet 
                                on t_time_sheet.ID=t_time_sheet_task.TS_ID
                                where t_time_sheet.TS_STATUS= @status
                                and t_time_sheet.TS_TIMESTAMP>=@startTimeStamp
                                and t_time_sheet.TS_TIMESTAMP<=@endTimeStamp
                                "+ condationSqlBuilder.ToString() + "group by t_time_sheet_task.TST_USER_ID,t_time_sheet_task.P_ID,t_time_sheet_task.P_NAME";

            var userProjectTimeList = db.Query<T_TIME_SHEET_TASK>(querySql, param, null, false, null, null).ToList();
            return userProjectTimeList;

        }
    }
}
