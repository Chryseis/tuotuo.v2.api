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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Repository
{
    public class TimeSheetRepository: ITimeSheetRepository
    {
        private T_TIME_SHEET_BLL _timeSheetBll;
        private T_TIME_SHEET_TASK_BLL _timeSheetTaskBll;

        private IUserRepository _userRepository;
        private IProjectRepository _projectRepository;
        private ITeamRepository _teamRepository;

        public TimeSheetRepository(T_TIME_SHEET_BLL timeSheetBll, T_TIME_SHEET_TASK_BLL timeSheetTaskBll, 
            IUserRepository userRepository, IProjectRepository projectRepository, ITeamRepository teamRepository)
        {
            this._timeSheetBll = timeSheetBll;
            this._timeSheetTaskBll = timeSheetTaskBll;

            this._userRepository = userRepository;
            this._projectRepository = projectRepository;
            this._teamRepository = teamRepository;
        }

        public TimeSheetRepoModel GetTimeSheetDetail(string currentUserMail, long currentTimeStamp,bool throwException=true)
        {
            TimeSheetRepoModel timeSheetRepoModel = null;
            var timeSheet = this.getTimeSheetInfo(currentUserMail,currentTimeStamp);
            if (timeSheet == null)
            {
                if (throwException)
                {
                    Enforce.Throw(new LogicErrorException("当前TimeSheet不存在"));
                }
                return timeSheetRepoModel;
            }
            timeSheetRepoModel = new TimeSheetRepoModel();
            timeSheetRepoModel.info = timeSheet;
            timeSheetRepoModel.tasks = this.getTimeSheetTaskList(timeSheet.ID);
            return timeSheetRepoModel;
        }

        public List<TimeSheetRepoModel> GetCheckTimeSheetRepoModelList(int userID, long startTimeStamp, long endTimeStamp,
            List<int> selectTeamIDList, List<int> selectUserIDList, List<int> selectStatusList,
            int from, int to,out long total)
        {
            total = 0;
            List<TimeSheetRepoModel> list = new List<TimeSheetRepoModel>();

            var tuple=this.checkQueryTeamIDAndUserIDAndProjectID(userID, selectTeamIDList, selectUserIDList, null);
            selectUserIDList = tuple.Item2;

            var queryTimeSheet = new DapperExQuery<T_TIME_SHEET>();
            if (selectUserIDList != null && selectUserIDList.Count > 0)
            {
                queryTimeSheet.AndWhere(s => s.TS_USER_ID, OperationMethod.In, selectUserIDList.ToArray());
            }
            else
            {
                return list;
            }     
            if (selectStatusList!=null && selectStatusList.Count>0)
            {
                queryTimeSheet.AndWhere(s => s.TS_STATUS, OperationMethod.In, selectStatusList.ToArray());
            }
            queryTimeSheet.AndWhere(s => s.TS_TIMESTAMP, OperationMethod.GreaterOrEqual, startTimeStamp);
            queryTimeSheet.AndWhere(s => s.TS_TIMESTAMP, OperationMethod.LessOrEqual, endTimeStamp);

            var timeSheetInfoModelList =Mapper.Map<List<T_TIME_SHEET>, List<TimeSheetInfoModel>>(
                this._timeSheetBll.GetListByRowNumber(queryTimeSheet, "ID ASC", from, to, out total));

            var queryTimeSheetTask = new DapperExQuery<T_TIME_SHEET_TASK>();
            if (timeSheetInfoModelList!=null && timeSheetInfoModelList.Count>0)
            {
                queryTimeSheetTask.AndWhere(s => s.TS_ID, OperationMethod.In, timeSheetInfoModelList.Select(s => s.ID).ToArray());
            }
            
            var timeSheetTaskModelList = Mapper.Map<List<T_TIME_SHEET_TASK>, List<TimeSheetTaskModel>>(
                this._timeSheetTaskBll.GetList(queryTimeSheetTask));
            list = timeSheetInfoModelList.Select(s => new TimeSheetRepoModel()
            {
                info = s,
                tasks= timeSheetTaskModelList.Where(m=>m.sheetID.Equals(s.ID)).ToList()
            }).ToList();
            return list;
        }

        public TimeSheetReportModel GetReportTimeSheetList(int userID, long startTimeStamp, long endTimeStamp,
            List<int> selectTeamIDList, List<int> selectUserIDList, List<int> selectProjectIDList,
            int from, int to, out long total)
        {
            total = 0;
            TimeSheetReportModel model = new TimeSheetReportModel();
            var tuple=this.checkQueryTeamIDAndUserIDAndProjectID(userID, selectTeamIDList, selectUserIDList, selectProjectIDList);
            selectTeamIDList = tuple.Item1;
            selectUserIDList = tuple.Item2;
            selectProjectIDList = tuple.Item3;

            model.totalTime = this._timeSheetTaskBll.GetReportTotalTime(startTimeStamp, endTimeStamp, selectTeamIDList,
                selectUserIDList, selectProjectIDList);
            model.projectCount = selectProjectIDList.Count;
            model.teamMemberCount = selectUserIDList.Count;
            var userList=this._timeSheetTaskBll.GetReportPageList(selectTeamIDList, selectUserIDList, selectProjectIDList,from,to,out total);
            List<T_TIME_SHEET_TASK> userProjectTimeList = new List<T_TIME_SHEET_TASK>();
            if (userList.Count>0)
            {
                userProjectTimeList = this._timeSheetTaskBll.GetReportUserProjectTime(
                startTimeStamp, endTimeStamp, userList.Select(s => s.ID).Distinct().ToList(), selectProjectIDList);
            }
            model.userInfos = userList.Select(s => new TimeSheetReportUserModel()
            {
                userID = s.ID,
                userMail = s.U_EMAIL,
                userName = s.U_USER_NAME,
                timeSheetTimeInfos = userProjectTimeList.Where(m=>m.TST_USER_ID.Equals(s.ID)).Select(m => new TimeSheetReportProjectModel()
                {
                    projectID=m.P_ID,
                    projectName=m.P_NAME,
                    totalTime=m.TST_TIME
                }).ToList()
            }).ToList();
            
            return model;
        }

        public bool CreateTimeSheet(string currentUserMail,long currentTimeStamp)
        {
            var currentUserInfo = this._userRepository.GetUser(currentUserMail);
            if (currentUserInfo==null || currentUserInfo.info == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户信息不存在"));
            }
            TimeSheetInfoModel info = new TimeSheetInfoModel();
            info.userID = currentUserInfo.info.userID;
            info.userMail = currentUserInfo.info.mail;
            info.userName = currentUserInfo.info.userName;
            info.timeSheetTimeStamp = currentTimeStamp;
            info.timeSheetDate = DateTimeUtils.CreateDateTime(currentTimeStamp);
            info.status = (int)TimeSheetStatus.UnFillOut;
            info.createUserMail = currentUserMail;
            info.createTime = DateTime.Now;
            return this._timeSheetBll.Add(Mapper.Map<TimeSheetInfoModel, T_TIME_SHEET>(info));
        }

        public List<TimeSheetTaskModel> CreateTimeSheetTasks(int sheetID, List<CreateTimeSheetTaskModel> tasks, string currentUserMail)
        {
            List<TimeSheetTaskModel> insertedTask = new List<TimeSheetTaskModel>();
            var currentUserInfo = this._userRepository.GetUser(currentUserMail);
            if (currentUserInfo == null || currentUserInfo.info == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户信息不存在"));
            }
            var currentSheetInfoModel = this.getTimeSheetInfo(sheetID);
            if (currentSheetInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("不存在该TimeSheet"));
            }
            if (!currentSheetInfoModel.userMail.Equals(currentUserMail))
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet不属于该用户"));
            }
            if (currentSheetInfoModel.status.Equals((int)TimeSheetStatus.Checked))
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet已被审批,不能再修改"));
            }
            var currentUserProjectList = this._projectRepository.LoadProjectsByUserMail(currentUserMail);
            var fitTasks = tasks.Where(s => currentUserProjectList.Exists(m => m.projectID.Equals(s.projectID))).ToList();
            DateTime currentDate = DateTime.Now;
            List<TimeSheetTaskModel> list = fitTasks.Select(s => new TimeSheetTaskModel()
            {
                sheetID = sheetID,
                projectID = s.projectID,
                projectName = currentUserProjectList.Single(m => m.projectID.Equals(s.projectID)).projectName,
                detail = s.detail,
                time = s.time,
                userID = currentUserInfo.info.userID,
                userMail = currentUserInfo.info.mail,
                userName = currentUserInfo.info.userName,
                createUserMail = currentUserInfo.info.mail,
                createTime = currentDate
            }).ToList();
            if (currentSheetInfoModel.status.Equals((int)TimeSheetStatus.Submitted))
            {
                currentSheetInfoModel.submitTime = currentDate;
            }
            var userList = Mapper.Map<List<TimeSheetTaskModel>, List<T_TIME_SHEET_TASK>>(list);
            if (this._timeSheetTaskBll.AddTaskList(userList,Mapper.Map<TimeSheetInfoModel,T_TIME_SHEET>(currentSheetInfoModel)))
            {
                insertedTask= Mapper.Map<List<T_TIME_SHEET_TASK>, List<TimeSheetTaskModel>>(userList);
            }
            return insertedTask;

        }

        public void DeleteTimeSheetTask(int taskID, string currentUserMail)
        {
            var taskInfoModel = this.getTimeSheetTaskModel(taskID);
            if (taskInfoModel==null)
            {
                Enforce.Throw(new LogicErrorException("不存在该Task"));
            }
            if (!taskInfoModel.userMail.Equals(currentUserMail))
            {
                Enforce.Throw(new UnAuthorizeException("暂无权限删除该Task"));
            }
            this._timeSheetTaskBll.Delete(new DapperExQuery<T_TIME_SHEET_TASK>().AndWhere(s=>s.ID,OperationMethod.Equal,taskID));
        }

        public bool ModifyTimeSheetTask(int taskID, string detail, int selectProjectID, decimal time, string currentUserMail)
        {
            var taskInfoModel = this.getTimeSheetTaskModel(taskID);
            if (taskInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("不存在该Task"));
            }
            if (!taskInfoModel.userMail.Equals(currentUserMail))
            {
                Enforce.Throw(new UnAuthorizeException("暂无权限修改该Task"));
            }
            var currentSheetInfoModel = this.getTimeSheetInfo(taskInfoModel.sheetID);
            if (currentSheetInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("不存在所属的TimeSheet"));
            }
            if (!currentSheetInfoModel.userMail.Equals(currentUserMail))
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet不属于该用户"));
            }
            if (currentSheetInfoModel.status.Equals((int)TimeSheetStatus.Checked))
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet已被审批,不能再修改"));
            }
            var currentUserProjectList = this._projectRepository.LoadProjectsByUserMail(currentUserMail);
            if (!currentUserProjectList.Exists(s=>s.projectID.Equals(selectProjectID)))
            {
                Enforce.Throw(new LogicErrorException("当前用户不属于选择的项目"));
            }
            taskInfoModel.detail = detail;
            taskInfoModel.projectID = selectProjectID;
            taskInfoModel.projectName = currentUserProjectList.Single(s => s.projectID.Equals(selectProjectID)).projectName;
            taskInfoModel.time = time;
            
            if (currentSheetInfoModel.status.Equals((int)TimeSheetStatus.Submitted))
            {
                currentSheetInfoModel.submitTime = DateTime.Now;
            }
            return this._timeSheetTaskBll.ModifyTask(Mapper.Map<TimeSheetTaskModel, T_TIME_SHEET_TASK>(taskInfoModel),
                Mapper.Map<TimeSheetInfoModel, T_TIME_SHEET>(currentSheetInfoModel));

        }


        public bool ApproveTimeSheet(int sheetID, TimeSheetResultStatus result,string comment,long viewTimeStamp, string currentUserMail)
        {
            var currentUserInfo = this._userRepository.GetUser(currentUserMail);
            if (currentUserInfo == null || currentUserInfo.info == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户信息不存在"));
            }
            var currentSheetInfoModel = this.getTimeSheetInfo(sheetID);
            if (currentSheetInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("不存在该TimeSheet"));
            }
            if (currentSheetInfoModel.status.Equals((int)TimeSheetStatus.UnFillOut))
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet尚未填写"));
            }

            if (currentSheetInfoModel.status.Equals((int)TimeSheetStatus.Checked))
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet已被审批"));
            }

            if (currentSheetInfoModel.submitTime.HasValue && currentSheetInfoModel.submitTime.Value.ToTimeStamp()> viewTimeStamp)
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet已被修改，请重新加载审批"));
            }

            var approvedUserInfo= this._userRepository.GetUser(currentSheetInfoModel.userMail);
            if (approvedUserInfo == null || approvedUserInfo.info == null)
            {
                Enforce.Throw(new LogicErrorException("被审核的TimeSheet所属用户信息不存在"));
            }

            if (!currentUserInfo.roleList.Exists(s =>
             s.roleType.Equals(RoleType.Team)
             && (s.roleCode.Equals(RoleCode.Manager) || s.roleCode.Equals(RoleCode.Owner))
             && approvedUserInfo.roleList.Exists(m => m.roleType.Equals(RoleType.Team) && m.relationID.Equals(s.relationID))
            ))
            {
                Enforce.Throw(new UnAuthorizeException("当前用户无权限审批改TimeSheet"));
            }
            currentSheetInfoModel.status = (int)TimeSheetStatus.Checked;
            currentSheetInfoModel.approvalUserID = currentUserInfo.info.userID;
            currentSheetInfoModel.approvalUserMail= currentUserInfo.info.mail;
            currentSheetInfoModel.approvalUserName = currentUserInfo.info.userName;
            currentSheetInfoModel.approvalTime = DateTime.Now;
            currentSheetInfoModel.approvalComment = comment;
            currentSheetInfoModel.approvalResult = (int)result;

            return this._timeSheetBll.Update(Mapper.Map<TimeSheetInfoModel,T_TIME_SHEET>(currentSheetInfoModel));

        }

        public bool SubmitTimeSheet(int sheetID, string currentUserMail)
        {
            var currentSheetInfoModel = this.getTimeSheetInfo(sheetID);
            if (currentSheetInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("不存在该TimeSheet"));
            }
            if (!currentSheetInfoModel.userMail.Equals(currentUserMail))
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet不属于该用户"));
            }
            if (currentSheetInfoModel.status.Equals((int)TimeSheetStatus.Checked))
            {
                Enforce.Throw(new LogicErrorException("该TimeSheet已被审批"));
            }
            currentSheetInfoModel.status = (int)TimeSheetStatus.Submitted;
            currentSheetInfoModel.submitTime = DateTime.Now;
            return this._timeSheetBll.Update(Mapper.Map<TimeSheetInfoModel, T_TIME_SHEET>(currentSheetInfoModel));
        }

        public List<TimeSheetQueryModel> GetQueryParams(int  userID)
        {
            var list = new List<TimeSheetQueryModel>();
            var teamRepoModelList=this.loadTeamRepoManagerList(userID);
            var userIDList=teamRepoModelList
                .Select(s => s.members.Select(m => m.userID).ToList()).Aggregate((result, next) =>
                {
                    result.AddRange(next);
                    return result;
                })
                .Distinct()
                .ToList();
            var userProjectDict=this._projectRepository.LoadProjectInfosByUserArray(userIDList);
            list=teamRepoModelList.Select(s => new TimeSheetQueryModel()
            {
                teamID=s.info.teamID,
                teamName=s.info.teamName,
                teamMembers=s.members.Select(m=>new TimeSheetQueryUserModel()
                {
                    userID=m.userID,
                    userMail=m.memberMail,
                    userName=m.memberName,
                    userProjects= userProjectDict.ContainsKey(m.userID)? 
                    userProjectDict[m.userID]
                    .Select(n=>new TimeSheetQueryProjectModel()
                    {
                         projectID=n.projectID,
                         projectName=n.projectName
                    })
                    .ToList(): new List<TimeSheetQueryProjectModel>()
                }).ToList()
            }).ToList();
            return list;

        }



        #region 私有方法


        /// <summary>
        /// 根据邮箱和时间戳获取TimeSheet详情
        /// </summary>
        /// <param name="currentUserMail"></param>
        /// <param name="currentTimeStamp"></param>
        /// <returns></returns>
        private TimeSheetInfoModel getTimeSheetInfo(string currentUserMail,long currentTimeStamp)
        {
            TimeSheetInfoModel  infoModel = null;
            var timeSheet = this._timeSheetBll.GetEntity(new DapperExQuery<T_TIME_SHEET>()
                .AndWhere(s => s.TS_TIMESTAMP, OperationMethod.Equal, currentTimeStamp)
                .AndWhere(s=>s.TS_USER_MAIL,OperationMethod.Equal, currentUserMail)
                );
            if (timeSheet != null)
            {
                infoModel = Mapper.Map<T_TIME_SHEET, TimeSheetInfoModel>(timeSheet);
            }
            return infoModel;
        }


        /// <summary>
        /// 根据Sheet ID获取TimeSheet详情
        /// </summary>
        /// <param name="sheetID"></param>
        /// <returns></returns>
        private TimeSheetInfoModel getTimeSheetInfo(int sheetID)
        {
            TimeSheetInfoModel infoModel = null;
            var timeSheet = this._timeSheetBll.GetEntity(new DapperExQuery<T_TIME_SHEET>()
                .AndWhere(s => s.ID, OperationMethod.Equal, sheetID)
                );
            if (timeSheet != null)
            {
                infoModel = Mapper.Map<T_TIME_SHEET, TimeSheetInfoModel>(timeSheet);
            }
            return infoModel;
        }

        /// <summary>
        /// 根据Task ID获取TimeSheetTask详情
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        private TimeSheetTaskModel getTimeSheetTaskModel(int taskID)
        {
            TimeSheetTaskModel model = null;
            var taskModel = this._timeSheetTaskBll.GetEntity(new DapperExQuery<T_TIME_SHEET_TASK>().AndWhere(s => s.ID, OperationMethod.Equal, taskID));
            if (taskModel != null)
            {
                model = Mapper.Map<T_TIME_SHEET_TASK, TimeSheetTaskModel>(taskModel);
            }
            return model;
        }


        /// <summary>
        /// 根据Sheet ID获取 Task列表
        /// </summary>
        /// <param name="timeSheetID"></param>
        /// <returns></returns>
        private List<TimeSheetTaskModel> getTimeSheetTaskList(int timeSheetID)
        {

            var tasks = this._timeSheetTaskBll
                .GetList(new DapperExQuery<T_TIME_SHEET_TASK>().AndWhere(s => s.TS_ID, OperationMethod.Equal, timeSheetID))
                .Select(s => Mapper.Map<T_TIME_SHEET_TASK, TimeSheetTaskModel>(s))
                .ToList();
            return tasks;
        }

        /// <summary>
        /// 获取用户管理的团队(拥有者或者管理员)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private List<TeamRepoModel> loadTeamRepoManagerList(int userID)
        {
            var teamRepoModelList = this._teamRepository.LoadTeamRepoList(userID,1)
            .Where(s => s.members.Exists(m => m.userID.Equals(userID) && (m.roleCode.Equals(RoleCode.Owner) || m.roleCode.Equals(RoleCode.Manager))))
            .ToList();
            return teamRepoModelList;
        }


        /// <summary>
        /// 校验查询审批和报表的参数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="selectTeamIDList"></param>
        /// <param name="selectUserIDList"></param>
        /// <param name="selectProjectIDList"></param>
        private Tuple<List<int>, List<int>, List<int>> checkQueryTeamIDAndUserIDAndProjectID(int userID, List<int> selectTeamIDList, List<int> selectUserIDList, List<int> selectProjectIDList)
        {
            var teamRepoModelList = this.loadTeamRepoManagerList(userID);

            if (selectTeamIDList != null && selectTeamIDList.Count > 0)
            {
                selectTeamIDList = selectTeamIDList.Where(s => teamRepoModelList.Exists(m => m.info.teamID.Equals(s))).ToList();
            }
            else
            {
                selectTeamIDList = teamRepoModelList.Select(s => s.info.teamID).Distinct().ToList();
            }

            if (selectUserIDList != null && selectUserIDList.Count > 0)
            {
                selectUserIDList = selectUserIDList.Where(s => teamRepoModelList.Exists(m => m.members.Exists(n => n.userID.Equals(s)))).ToList();
            }
            else if (selectTeamIDList != null && selectTeamIDList.Count > 0)
            {
               
                selectUserIDList = teamRepoModelList
                    .Where(s => selectTeamIDList.Contains(s.info.teamID))
                    .Select(s => s.members.Select(m => m.userID).ToList())
                    .Aggregate((result, next) =>
                    {
                        result.AddRange(next.Where(s => !result.Contains(s)));
                        return result;
                    });
            }
            else
            {
                selectUserIDList = teamRepoModelList.Select(s => s.members.Select(m => m.userID).ToList())
                    .Aggregate((result, next) =>
                    {
                        result.AddRange(next.Where(s => !result.Contains(s)));
                        return result;
                    });
            }
            var userProjectDict = this._projectRepository.LoadProjectInfosByUserArray(selectUserIDList);
            if (selectProjectIDList != null && selectProjectIDList.Count > 0)
            {
                selectProjectIDList = selectProjectIDList.Where(s => userProjectDict.Values.Count(m=>m.Exists(n=>n.projectID.Equals(s)))>0).ToList();
            }
            else
            {
                selectProjectIDList = userProjectDict.Select(s => s.Value.Select(m => m.projectID).ToList())
                                        .Aggregate(new List<int>(),(result, next) =>
                                        {
                                            result.AddRange(next.Where(s => !result.Contains(s)));
                                            return result;
                                        });

            }
            var tuple = new Tuple<List<int>, List<int>, List<int>>(selectTeamIDList, selectUserIDList, selectProjectIDList);
            return tuple;

        }

        #endregion


       
    }
}
