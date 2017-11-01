using AutoMapper;
using Mintcode.TuoTuo.v2.BLL;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
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
    public class TaskRepository : ITaskRepository
    {
        private T_TASK_BLL _taskBll;
        private T_TASK_LOG_BLL _taskLogBll;
        private ITeamRepository _teamRepository;
        private IProjectRepository _projectRepository;
        private IBacklogRepository _backlogRepository;
        private IUserRepository _userRepository;

        public TaskRepository(T_TASK_BLL taskBll, T_TASK_LOG_BLL taskLogBll, ITeamRepository teamRepository, IProjectRepository projectRepository, IBacklogRepository backlogRepository, IUserRepository userRepository)
        {
            this._taskBll = taskBll;
            this._taskLogBll = taskLogBll;
            this._teamRepository = teamRepository;
            this._projectRepository = projectRepository;
            this._backlogRepository = backlogRepository;
            this._userRepository = userRepository;
        }

        public int CreateTask(int backLogID, int teamID, string title, string content, string assignedEmail, string typeName, int time, int state, string currentUserMail)
        {
            assignedEmail = string.IsNullOrEmpty(assignedEmail) ? assignedEmail : assignedEmail.ToLower();
            var currentUserRepoModel = this._userRepository.GetUser(currentUserMail);
            if (currentUserRepoModel == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户已被删除"));
            }

            var assignedUserRepoModel = this._userRepository.GetUser(assignedEmail);
            if (assignedUserRepoModel == null)
            {
                Enforce.Throw(new LogicErrorException("指派的用户已被删除"));
            }

            var backLogInfo = this._backlogRepository.GetBacklogInfoModel(teamID, backLogID);
            //TeamRepoModel teamRepoModel = _teamRepository.GetTeamDetail(teamID);
            //if (!teamRepoModel.members.Any(A => A.memberMail == currentUserMail))
            //{
            //    Enforce.Throw(new LogicErrorException("该用户不属于当前团队"));
            //}

            ProjectRepoModel projectRepoModel = _projectRepository.GetProjectDetail(backLogInfo.projectID);
            if (!projectRepoModel.members.Any(A => A.memberMail.ToLower() == assignedEmail))
            {
                Enforce.Throw(new LogicErrorException("该用户不属于当前项目"));
            }

            TaskInfoModel taskInfoModel = new TaskInfoModel();
            taskInfoModel.backLogID = backLogID;
            taskInfoModel.projectID = backLogInfo.projectID;
            taskInfoModel.projectName = backLogInfo.projectName;
            taskInfoModel.title = title;
            taskInfoModel.content = content;
            taskInfoModel.assignedName = assignedUserRepoModel.info.userName;
            taskInfoModel.assignedEmail = assignedEmail;
            taskInfoModel.typeName = typeName;
            taskInfoModel.time = time;
            taskInfoModel.state = state;
            taskInfoModel.currentUserMail = currentUserMail;
            taskInfoModel.createTime = DateTime.Now;
            return this._taskBll.CreateTask(AutoMapper.Mapper.Map<TaskInfoModel, T_TASK>(taskInfoModel), currentUserRepoModel.info.userName);
        }

        public bool ModifyTask(int taskId, int projectID, int teamID, string title, string content, string assignedEmail, string typeName, int time, int state, string currentUserMail)
        {
            assignedEmail = string.IsNullOrEmpty(assignedEmail) ? assignedEmail : assignedEmail.ToLower();
            var currentUserRepoModel = this._userRepository.GetUser(currentUserMail);
            if (currentUserRepoModel == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户已被删除"));
            }
            string assignedUserName = null;
            if (assignedEmail != null)
            {
                var assignedUserRepoModel = this._userRepository.GetUser(assignedEmail);
                if (assignedUserRepoModel == null)
                {
                    Enforce.Throw(new LogicErrorException("指派的用户已被删除"));
                }
                assignedUserName = assignedUserRepoModel.info.userName;
            }

            T_TASK task = _taskBll.GetEntity(new DapperExQuery<T_TASK>()
               .AndWhere(s => s.ID, OperationMethod.Equal, taskId));
            if (task == null)
            {
                Enforce.Throw(new LogicErrorException("当前任务不存在"));
            }
            //TeamRepoModel teamRepoModel = _teamRepository.GetTeamDetail(teamID);
            //if (!teamRepoModel.members.Any(A => A.memberMail == currentUserMail))
            //{
            //    Enforce.Throw(new LogicErrorException("该用户不属于当前团队"));
            //}
            if (projectID > 0)
            {
                ProjectRepoModel projectRepoModel = _projectRepository.GetProjectDetail(projectID);
                if (!projectRepoModel.members.Any(A => A.memberMail == assignedEmail))
                {
                    Enforce.Throw(new LogicErrorException("该用户不属于当前项目"));
                }
            }
            TaskInfoModel taskInfoModel = Mapper.Map<T_TASK, TaskInfoModel>(task);
            taskInfoModel.title = title != null ? title : taskInfoModel.title;
            taskInfoModel.content = content != null ? content : taskInfoModel.content;
            taskInfoModel.assignedName = assignedEmail != null ? assignedUserName : taskInfoModel.assignedName;
            taskInfoModel.assignedEmail = assignedEmail != null ? assignedEmail : taskInfoModel.assignedEmail;
            taskInfoModel.typeName = typeName != null ? typeName : taskInfoModel.typeName;
            taskInfoModel.time = time != 0 ? time : taskInfoModel.time;
            taskInfoModel.state = state;
            return _taskBll.ModifyTask(AutoMapper.Mapper.Map<TaskInfoModel, T_TASK>(taskInfoModel), currentUserRepoModel.info.userName, currentUserMail, DateTime.Now);
        }

        public bool ModifyTaskState(int taskID, int teamID, int state, string currentUserMail)
        {
            var currentUserRepoModel = this._userRepository.GetUser(currentUserMail);
            if (currentUserRepoModel == null)
            {
                Enforce.Throw(new LogicErrorException("当前用户已被删除"));
            }

            T_TASK task = _taskBll.GetEntity(new DapperExQuery<T_TASK>()
              .AndWhere(s => s.ID, OperationMethod.Equal, taskID));
            if (task == null)
            {
                Enforce.Throw(new LogicErrorException("当前任务不存在"));
            }
            //TeamRepoModel teamRepoModel = _teamRepository.GetTeamDetail(teamID);
            //if (!teamRepoModel.members.Any(A => A.memberMail == currentUserMail))
            //{
            //    Enforce.Throw(new LogicErrorException("该用户不属于当前团队"));
            //}
            TaskInfoModel taskInfoModel = Mapper.Map<T_TASK, TaskInfoModel>(task);
            taskInfoModel.state = state;
            return _taskBll.ModifyTask(AutoMapper.Mapper.Map<TaskInfoModel, T_TASK>(taskInfoModel), currentUserRepoModel.info.userName, currentUserMail, DateTime.Now);
        }

        public TaskRepoModel getTaskInfo(int taskID)
        {
            TaskRepoModel taskRepoModel = new TaskRepoModel();
            T_TASK task = _taskBll.GetEntity(new DapperExQuery<T_TASK>()
             .AndWhere(s => s.ID, OperationMethod.Equal, taskID));
            if (task == null)
            {
                Enforce.Throw(new LogicErrorException("当前任务不存在"));
            }

            TaskInfoModel taskInfoModel = Mapper.Map<T_TASK, TaskInfoModel>(task);
            taskRepoModel.info = taskInfoModel;
            taskRepoModel.logs = this.getTaskLog(taskID);
            return taskRepoModel;
        }
        
     
        public List<TaskInfoModel> GetTaskList(List<int> backLogIDList)
        {
            List<TaskInfoModel> taskList = new List<TaskInfoModel>();
            if (backLogIDList!=null && backLogIDList.Count>0)
            {
                taskList = this._taskBll
                    .GetList(new DapperExQuery<T_TASK>().AndWhere(s => s.T_BACKLOG_ID, OperationMethod.In, backLogIDList.ToArray()))
                    .Select(s => Mapper.Map<T_TASK, TaskInfoModel>(s))
                    .ToList();
            }
            

            return taskList;
        }

        public List<TaskInfoModel> getMyCompleteTaskList(String currentUserMail,DateTime startTime,DateTime endTime)
        {
            var taskList=this._taskBll
               .GetList(new DapperExQuery<T_TASK>().AndWhere(s => s.T_ASSIGNED_EMAIL, OperationMethod.Equal, currentUserMail)
               .AndWhere(s => s.T_STATE, OperationMethod.Equal, TaskState.DONE)
               .AndWhere(s => s.CREATE_TIME, OperationMethod.GreaterOrEqual, startTime)
               .AndWhere(s => s.CREATE_TIME, OperationMethod.Less, endTime))
               .Select(s => Mapper.Map<T_TASK, TaskInfoModel>(s))
               .ToList();

            return taskList;
        }

        public List<TaskInfoModel> getTaskList(int backLogID)
        {
            var taskList = this._taskBll
               .GetList(new DapperExQuery<T_TASK>().AndWhere(s => s.T_BACKLOG_ID, OperationMethod.Equal, backLogID))
               .Select(s => Mapper.Map<T_TASK, TaskInfoModel>(s))
               .ToList();

            return taskList;
        }

        public List<TaskLogModel> getTaskLog(int taskID)
        {
            var taskLogs = this._taskLogBll
               .GetList(new DapperExQuery<T_TASK_LOG>().AndWhere(s => s.T_TASK_ID, OperationMethod.Equal, taskID))
               .Select(s => Mapper.Map<T_TASK_LOG, TaskLogModel>(s))
               .OrderByDescending(o=> o.createTime)
               .ToList();
            return taskLogs;
        }
    }
}
