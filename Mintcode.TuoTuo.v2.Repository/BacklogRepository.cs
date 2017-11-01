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
    public class BacklogRepository: IBacklogRepository
    {
        private IScrumRepository _scrumRepository;
        private IProjectRepository _projectRepository;
        private IUserRepository _userRepository;
        private T_BACKLOG_BLL _backlogBll;
        private ITaskRepository _taskRepository;

        public BacklogRepository(IScrumRepository scrumRepository, IProjectRepository projectRepository, 
            IUserRepository userRepository,
            T_BACKLOG_BLL backlogBll)
        {
            this._scrumRepository = scrumRepository;
            this._projectRepository = projectRepository;
            this._userRepository = userRepository;
            this._backlogBll = backlogBll;
        }

        public List<BacklogInfoModel> GetBacklogInfoList(int teamID, int sprintID)
        {           
            var query = new DapperExQuery<T_BACKLOG>();
            query.AndWhere(s=>s.T_TEAM_ID,OperationMethod.Equal,teamID);
            query.AndWhere(s => s.B_STATE, OperationMethod.NotEqual, (int)BacklogState.REMOVE);
            string orderString = string.Empty;
            sprintID = sprintID < 0 ? 0 : sprintID;
            if (sprintID > 0 && this._scrumRepository.GetSprintInfoByTeamIDAndSprintID(teamID, sprintID) == null)
            {
                Enforce.Throw(new LogicErrorException("当前Sprint不存在"));
            }
            query.AndWhere(s => s.R_SPRINT_ID, OperationMethod.Equal, sprintID);
            if (sprintID > 0)
            {
                orderString = "B_LEVEL ASC";
            }
            else
            {
                orderString = "CREATE_TIME DESC";
            }
            
            var list = this._backlogBll.GetList(query, orderString);
            var backlogInfoList=Mapper.Map<List<T_BACKLOG>, List<BacklogInfoModel>>(list);
            return backlogInfoList;
        }

        public List<BacklogRepoModel> GetBacklogRepoList(int teamID, int sprintID)
        {
            var currentSprintInfoModel=this._scrumRepository.GetSprintInfoByTeamIDAndSprintID(teamID,sprintID);
            if (currentSprintInfoModel==null)
            {
                Enforce.Throw(new LogicErrorException("查询的Sprint不存在"));
            }
            if (currentSprintInfoModel.state == 0)
            {
                Enforce.Throw(new LogicErrorException("查询的Sprint不是团队的当前Sprint"));
            }
            List<BacklogRepoModel> backlogRepoModelList = new List<BacklogRepoModel>();
            var backlogInfoList=this.GetBacklogInfoList(teamID, sprintID);
            var backlogIDList = backlogInfoList.Select(s => s.ID).ToList();
            var taskInfoList = this._taskRepository.GetTaskList(backlogIDList);
            backlogRepoModelList=backlogInfoList.Select(s => new BacklogRepoModel()
            {
                info=s,
                tasks= taskInfoList.Where(m=>m.backLogID.Equals(s.ID)).ToList()
            }).ToList();
            return backlogRepoModelList;
        }

        public bool SetBackLogsSprint(int teamID, int sprintID, List<int> backLogIDs)
        {
            bool result = true;
            if (sprintID<0 || (sprintID > 0 && this._scrumRepository.GetSprintInfoByTeamIDAndSprintID(teamID, sprintID) == null))
            {
                Enforce.Throw(new LogicErrorException("当前Sprint不存在"));
            }
            if (backLogIDs!=null && backLogIDs.Count>0)
            {
                result = this._backlogBll.SetBackLogsSprint(teamID, sprintID, backLogIDs);
            }
            return result;
        }


        public BacklogInfoModel CreateBackLog(int teamID, string title, string content, string standard, string assignUserMail, int selectProjectID, int state, int? level,string createUserMail)
        {
            var projectInfoModel = this._projectRepository.GetProjectDetail(selectProjectID);
            if (projectInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("项目不存在"));
            }
            var userInfoModel = this._userRepository.GetUser(assignUserMail,1);
            if (userInfoModel==null)
            {
                Enforce.Throw(new LogicErrorException("负责人账号不存在"));
            }
            if (this._projectRepository.GetProjectMember(selectProjectID, userInfoModel.info.mail) ==null)
            {
                Enforce.Throw(new LogicErrorException("负责人不在项目中"));
            }           
            BacklogInfoModel backlogInfoModel = new BacklogInfoModel();
            backlogInfoModel.teamID = teamID;
            backlogInfoModel.title = title;
            backlogInfoModel.content = content;
            backlogInfoModel.standard = standard;
            backlogInfoModel.sprintID = 0;
            backlogInfoModel.assignUserMail = userInfoModel.info.mail;
            backlogInfoModel.assignUserName = userInfoModel.info.userName;
            backlogInfoModel.projectID = selectProjectID;
            backlogInfoModel.projectName = projectInfoModel.info.projectName;
            backlogInfoModel.state = state;
            backlogInfoModel.level = level;
            backlogInfoModel.createUserMail = createUserMail;
            backlogInfoModel.createTime = DateTime.Now;
            var backlog = Mapper.Map<BacklogInfoModel, T_BACKLOG>(backlogInfoModel);
            if (this._backlogBll.Add(backlog))
            {
                backlogInfoModel= Mapper.Map<T_BACKLOG, BacklogInfoModel>(backlog);
            }
            return backlogInfoModel;
        }

        public BacklogInfoModel ModifyBackLog(int teamID, int backlogID, string title, string content, string standard, string assignUserMail, int selectProjectID, int state, int? level)
        {
            var userInfoModel = this._userRepository.GetUser(assignUserMail, 1);
            if (userInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("负责人账号不存在"));
            }
  
            var projectRepoModel= this._projectRepository.GetProjectDetail(selectProjectID);
            if (projectRepoModel==null || projectRepoModel.info==null)
            {
                Enforce.Throw(new LogicErrorException("所选项目不存在"));
            }
            if (this._projectRepository.GetProjectMember(selectProjectID, assignUserMail) == null)
            {
                Enforce.Throw(new LogicErrorException("负责人不在所选项目中"));
            }

            BacklogInfoModel backlogInfoModel = this.GetBacklogInfoModel(teamID, backlogID);
            if (backlogInfoModel==null)
            {
                Enforce.Throw(new LogicErrorException("不存在该Backlog"));
            }
                   
            backlogInfoModel.title = title;
            backlogInfoModel.content = content;
            backlogInfoModel.standard = standard;
            backlogInfoModel.assignUserMail = userInfoModel.info.mail;
            backlogInfoModel.assignUserName = userInfoModel.info.userName;
            backlogInfoModel.projectID = selectProjectID;
            backlogInfoModel.projectName = projectRepoModel.info.projectName;
            backlogInfoModel.state = state;
            backlogInfoModel.level = level;
            if (this._backlogBll.Update(Mapper.Map<BacklogInfoModel, T_BACKLOG>(backlogInfoModel)))
            {
                return backlogInfoModel;
            }
            else
            {
                return null;
            }
        }


        public BacklogInfoModel GetBacklogInfoModel(int teamID, int backlogID)
        {
            BacklogInfoModel backlogInfoModel = null;
            var backlogEntity=this._backlogBll.GetEntity(new DapperExQuery<T_BACKLOG>().AndWhere(s=>s.ID,OperationMethod.Equal,backlogID));
            if (backlogEntity!=null)
            {
                backlogInfoModel = Mapper.Map<T_BACKLOG, BacklogInfoModel>(backlogEntity);
                if (!backlogInfoModel.teamID.Equals(teamID))
                {
                    Enforce.Throw(new LogicErrorException("当前团队不存在该Backlog"));   
                }
                if (backlogInfoModel.state.Equals((int)BacklogState.REMOVE))
                {
                    Enforce.Throw(new LogicErrorException("该Backlog已被删除"));
                }
            }
            return backlogInfoModel;
        }


        public bool DeleteBacklog(int teamID, int backlogID)
        {
            BacklogInfoModel backlogInfoModel = this.GetBacklogInfoModel(teamID, backlogID);
            if (backlogInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("不存在该Backlog"));
            }            
            backlogInfoModel.state = (int)BacklogState.REMOVE;
            return this._backlogBll.Update(Mapper.Map<BacklogInfoModel, T_BACKLOG>(backlogInfoModel));

        }


        public void SetTaskRepository(ITaskRepository taskRepository)
        {
            this._taskRepository = taskRepository;
        }

    }
}
