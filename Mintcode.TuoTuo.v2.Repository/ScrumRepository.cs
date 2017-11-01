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
    public class ScrumRepository : IScrumRepository
    {
        private T_RELEASE_BLL _relaseBll;
        private T_RELEASE_SPRINT_BLL _relaseSprintBll;
        private IBacklogRepository _backlogRepository;
        public ScrumRepository(T_RELEASE_BLL relaseBll, T_RELEASE_SPRINT_BLL relaseSprintBll)
        {
            this._relaseBll = relaseBll;
            this._relaseSprintBll = relaseSprintBll;
        }
        public ReleaseInfoModel CreateRelease(int teamID, string releaseName, string releaseSummary, string currentMail)
        {
            ReleaseInfoModel releaseInfoModel = new ReleaseInfoModel();
            releaseInfoModel.releaseName = releaseName;
            releaseInfoModel.releaseSummary = releaseSummary;
            releaseInfoModel.teamID = teamID;
            releaseInfoModel.createUserMail = currentMail;
            releaseInfoModel.createTime = DateTime.Now;
            var release = AutoMapper.Mapper.Map<ReleaseInfoModel, T_RELEASE>(releaseInfoModel);
            if (this._relaseBll.Add(release))
            {
                releaseInfoModel = Mapper.Map<T_RELEASE, ReleaseInfoModel>(release);
            }
            return releaseInfoModel;
        }

        public ReleaseInfoModel ViewRelease(int teamID, int releaseID)
        {
            return this.getReleaseInfoByTeamIDAndReleaseID(teamID, releaseID);
        }

        public List<ReleaseInfoModel> ListRelease(int teamID)
        {
            var releaseList=this._relaseBll.GetList(new DapperExQuery<T_RELEASE>().AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID));
            return AutoMapper.Mapper.Map<List<T_RELEASE>, List<ReleaseInfoModel>>(releaseList);       
        }

        public bool DeleteRelease(int teamID, int releaseID)
        {
            bool result = true;
            if (this.getReleaseInfoByTeamIDAndReleaseID(teamID, releaseID)!=null)
            {
                if (this.ListSprint(teamID, releaseID).Count>0)
                {
                    Enforce.Throw(new LogicErrorException("当前Release下存在Sprint，无法删除"));
                }
                result = this._relaseBll.DeleteRelease(releaseID);
            }          
            return result;
        }

        public SprintInfoModel CreateSprint(int teamID,int releaseID, DateTime startTime, DateTime endTime, string currentMail)
        {
            if (this.getReleaseInfoByTeamIDAndReleaseID(teamID,releaseID)==null)
            {
                Enforce.Throw(new LogicErrorException("不存在该Release"));
            }
            SprintInfoModel sprintInfoModel = new SprintInfoModel();
            sprintInfoModel.releaseID = releaseID;
            sprintInfoModel.startTime = startTime;
            sprintInfoModel.endTime = endTime;
            sprintInfoModel.state = 0;
            sprintInfoModel.createUserMail = currentMail;
            sprintInfoModel.createTime = DateTime.Now;

            var sprintInfoModelList =this.ListSprint(teamID, releaseID);
            var maxSprintNo = sprintInfoModelList.Count>0?sprintInfoModelList.Max(s => s.no):0;
            string lockKey = string.Format("Scrum_Team_{0}", teamID);

            lock (lockKey)
            {
                sprintInfoModelList = this.ListSprint(teamID, releaseID);
                maxSprintNo = sprintInfoModelList.Count > 0 ? sprintInfoModelList.Max(s => s.no) : 0;
                sprintInfoModel.no = maxSprintNo + 1;
                if (sprintInfoModelList.Count <= 0)
                {
                    sprintInfoModel.state = 1;
                }
                var sprint = AutoMapper.Mapper.Map<SprintInfoModel, T_RELEASE_SPRINT>(sprintInfoModel);
                if (this._relaseSprintBll.Add(sprint))
                {
                    sprintInfoModel = Mapper.Map<T_RELEASE_SPRINT, SprintInfoModel>(sprint);
                }
            }
            
            

            return sprintInfoModel;

           
        }
        public List<SprintInfoModel> ListSprint(int teamID, int releaseID)
        {
            if (this.getReleaseInfoByTeamIDAndReleaseID(teamID, releaseID)==null)
            {
                Enforce.Throw(new LogicErrorException("不存在该Release"));
            }
            var sprintList = this._relaseSprintBll.GetList(new DapperExQuery<T_RELEASE_SPRINT>()
                .AndWhere(s => s.R_RELEASE_ID, OperationMethod.Equal, releaseID));

            return AutoMapper.Mapper.Map<List<T_RELEASE_SPRINT>, List<SprintInfoModel>>(sprintList);
        }

        public bool SetCurrentSprint(int teamID, int sprintID)
        {
           
            SprintInfoModel sprintInfoModel = this.GetSprintInfoByTeamIDAndSprintID(teamID,sprintID);
            if (sprintInfoModel==null)
            {
                Enforce.Throw(new LogicErrorException("当前Sprint不存在"));
            }
            string lockKey = string.Format("Scrum_Team_{0}", teamID);
            lock (lockKey)
            {
                return this._relaseSprintBll.SetCurrentSprint(teamID, sprintID);
            }
            
        }

        public bool EditSprint(int teamID, int sprintID, DateTime startTime, DateTime endTime)
        {
            SprintInfoModel sprintInfoModel = this.GetSprintInfoByTeamIDAndSprintID(teamID, sprintID);
            if (sprintInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("当前Sprint不存在"));
            }
            sprintInfoModel.startTime = startTime;
            sprintInfoModel.endTime = endTime;
            return this._relaseSprintBll.Update(AutoMapper.Mapper.Map<SprintInfoModel,T_RELEASE_SPRINT>(sprintInfoModel));

        }


        public void DeleteSprint(int teamID, int sprintID)
        {
            SprintInfoModel sprintInfoModel = this.GetSprintInfoByTeamIDAndSprintID(teamID, sprintID);
            if (sprintInfoModel != null)
            {
                if (this._backlogRepository.GetBacklogInfoList(teamID, sprintID).Where(s=>!s.state.Equals(BacklogState.FAIL)).Count() > 0)
                {
                    Enforce.Throw(new LogicErrorException("当前Sprint下存在Backlog，无法删除"));
                }
                var sprintInfoModelList = this.ListSprint(teamID, sprintInfoModel.releaseID);
                var maxSprintNo = sprintInfoModelList.Count>0?sprintInfoModelList.Max(s => s.no):0;
                if (!sprintInfoModel.no.Equals(maxSprintNo))
                {
                    Enforce.Throw(new LogicErrorException("不能删除该Sprint"));  
                }
                string lockKey = string.Format("Scrum_Team_{0}", teamID);
                lock (lockKey)
                {
                    sprintInfoModelList = this.ListSprint(teamID, sprintInfoModel.releaseID);       
                    if (sprintInfoModelList.Count <=0)
                    {
                        Enforce.Throw(new LogicErrorException("当前Sprint已经被删除"));
                    }
                    maxSprintNo = sprintInfoModelList.Count > 0 ? sprintInfoModelList.Max(s => s.no) : 0;
                    if (!sprintInfoModel.no.Equals(maxSprintNo))
                    {
                        Enforce.Throw(new LogicErrorException("不能删除该Sprint"));
                    }
                    SprintInfoModel modifySprintInfoModel = 
                        sprintInfoModelList.OrderByDescending(s => s.no).Where(s => s.no < maxSprintNo).FirstOrDefault();
                    if (modifySprintInfoModel != null)
                    {
                        modifySprintInfoModel.state = 1;
                        this._relaseSprintBll.DeleteSprint(new DapperExQuery<T_RELEASE_SPRINT>()
                        .AndWhere(s => s.ID, OperationMethod.Equal, sprintID)
                        .AndWhere(s => s.R_NO, OperationMethod.Equal, maxSprintNo), Mapper.Map<SprintInfoModel, T_RELEASE_SPRINT>(modifySprintInfoModel));
                    }
                    else
                    {
                        this._relaseSprintBll.DeleteSprint(new DapperExQuery<T_RELEASE_SPRINT>()
                        .AndWhere(s => s.ID, OperationMethod.Equal, sprintID)
                        .AndWhere(s => s.R_NO, OperationMethod.Equal, maxSprintNo),null);
                    }
                    
                }
               
            }
        }


        public SprintInfoModel GetSprintInfoByTeamIDAndSprintID(int teamID, int sprintID)
        {
            SprintInfoModel sprintInfoModel = this.getSprintInfoBySprintID(sprintID);
            if (sprintInfoModel != null)
            {
                if (this.getReleaseInfoByTeamIDAndReleaseID(teamID, sprintInfoModel.releaseID) == null)
                {
                    Enforce.Throw(new LogicErrorException("当前团队不存在该Sprint"));
                }
            }
            return sprintInfoModel;
        }


        public List<ReleaseRepoModel> GetReleaseAndSprintList(int teamID)
        {
            List<ReleaseRepoModel> releaseRepoModelList = new List<ReleaseRepoModel>();    
            var releaseInfoList=this.ListRelease(teamID);
            if (releaseInfoList.Count>0)
            {
                int[] releaseIDArray = releaseInfoList.Select(s => s.ID).ToArray();
                var sprintEntityList = this._relaseSprintBll.GetList(
                    new DapperExQuery<T_RELEASE_SPRINT>().AndWhere(s => s.R_RELEASE_ID, OperationMethod.In, releaseIDArray));
                var sprintInfoList = Mapper.Map<List<T_RELEASE_SPRINT>, List<SprintInfoModel>>(sprintEntityList);
                releaseRepoModelList = releaseInfoList.Select(s => new ReleaseRepoModel()
                {
                    releaseInfo = s,
                    sprintInfoList = sprintInfoList.Where(m=>m.releaseID.Equals(s.ID)).ToList()
                }).ToList();
            }         
            return releaseRepoModelList;
            
        }

        public CurrentReleaseAndSprint GetCurrentReleaseAndSprint(int teamID)
        {
            CurrentReleaseAndSprint currentReleaseAndSprint = null;
            var currentReleaseRepoModel= this.GetReleaseAndSprintList(teamID).SingleOrDefault(s => s.sprintInfoList.Exists(m => m.state.Equals(1)));
            if (currentReleaseRepoModel!=null)
            {
                var currentSprintInfoModel = currentReleaseRepoModel.sprintInfoList.SingleOrDefault(s=>s.state.Equals(1));
                if (currentSprintInfoModel!=null)
                {
                    currentReleaseAndSprint = new CurrentReleaseAndSprint();
                    currentReleaseAndSprint.releaseID = currentReleaseRepoModel.releaseInfo.ID;
                    currentReleaseAndSprint.releaseName = currentReleaseRepoModel.releaseInfo.releaseName;
                    currentReleaseAndSprint.sprintID = currentSprintInfoModel.ID;
                    currentReleaseAndSprint.sprintNo = currentSprintInfoModel.no;
                    currentReleaseAndSprint.startTimestamp = currentSprintInfoModel.startTimestamp;
                    currentReleaseAndSprint.endTimestamp= currentSprintInfoModel.endTimestamp;
                }
            }
            return currentReleaseAndSprint;
        }

        public void SetBacklogRepository(IBacklogRepository backlogRepository)
        {
            this._backlogRepository = backlogRepository;
        }


        #region 私有方法

        /// <summary>
        /// 根据ID获取Sprint详情
        /// </summary>
        /// <param name="sprintID"></param>
        /// <returns></returns>
        private SprintInfoModel getSprintInfoBySprintID(int sprintID)
        {
            SprintInfoModel sprintInfoModel = null;
            var sprintEntity = this._relaseSprintBll.GetEntity(new DapperExQuery<T_RELEASE_SPRINT>().AndWhere(s=>s.ID,OperationMethod.Equal,sprintID));
            if (sprintEntity!=null)
            {
                sprintInfoModel = AutoMapper.Mapper.Map<T_RELEASE_SPRINT, SprintInfoModel>(sprintEntity);
            }
            return sprintInfoModel;
        }

        private ReleaseInfoModel getReleaseInfoByTeamIDAndReleaseID(int teamID,int releaseID)
        {
            ReleaseInfoModel releaseInfoModel = null;
            var releaseEntity = this._relaseBll.GetEntity(new DapperExQuery<T_RELEASE>().AndWhere(s => s.ID, OperationMethod.Equal, releaseID));
            if (releaseEntity != null)
            {
                releaseInfoModel = AutoMapper.Mapper.Map<T_RELEASE, ReleaseInfoModel>(releaseEntity);
                if (!releaseInfoModel.teamID.Equals(teamID))
                {
                    Enforce.Throw(new LogicErrorException("当前团队不存在该Release"));
                }
            }
            return releaseInfoModel;
        }


      

        #endregion

    }
}
