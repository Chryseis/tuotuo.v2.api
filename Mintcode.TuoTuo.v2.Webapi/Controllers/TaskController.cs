using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.Infrastructure.Util;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    [RoutePrefix("task")]
    public class TaskController : BaseController
    {

        private ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository,  IdentityService identityService)
            : base(identityService)
        {
            this._taskRepository = taskRepository;
        }

      


        /// <summary>
        /// task 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ListTask")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult ListTask([FromUri]ReqListTask req)
        {
            var response = new ResListTask();
            var backInfoList = this._taskRepository.getTaskList( req.backLogID);
            response.setResponse(ResStatusCode.OK, backInfoList, backInfoList.Count);
            return Ok(response);
        }
        /// <summary>
        /// task 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("MyCompleteTaskList")]
        [HttpGet]
        public IHttpActionResult ListTask([FromUri]ReqMyTask req)
        {
            var userRepoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var response = new ResListTask();

            DateTime date;
            if (req.currentTimeStamp <= 0)
            {
                date = DateTime.Now;
            }
            else
            {
                date = DateTimeUtils.CreateDateTime(req.currentTimeStamp);
            }
            DateTime startTime = new DateTime(date.Year, date.Month, date.Day);
            DateTime endTime = startTime.AddDays(1);
            var backInfoList = this._taskRepository.getMyCompleteTaskList(userRepoModel.mail,startTime, endTime);
            response.setResponse(ResStatusCode.OK, backInfoList, backInfoList.Count);
            return Ok(response);
        }

          /// <summary>
        /// 创建Task
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
          public IHttpActionResult Put(ReqCreateTask req)
        {
            var userRepoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var response = new ResViewTask();
            int taskId = this._taskRepository.CreateTask(req.backLogID, req.teamID,
               req.title, req.content, req.assignedEmail, req.typeName, req.time, req.state, userRepoModel.mail);
            if (taskId==0)
            {
                Enforce.Throw(new LogicErrorException("添加失败"));
            }
            return Get(new ReqViewTask() { taskID = taskId });
        }

        /// <summary>
        /// 更改Task
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult Post(ReqModifyTask req)
        {
            var response = new ResViewTask();
            var userRepoModel = this.GetUserModelFromCurrentClaimsIdentity();
            if (!this._taskRepository.ModifyTask(req.taskID, req.projectID, req.teamID,
               req.title, req.content, req.assignedEmail, req.typeName, req.time, req.state, userRepoModel.mail))
            {
                Enforce.Throw(new LogicErrorException("更改失败"));
            }
            return Get(new ReqViewTask() { taskID = req.taskID });
        }

        /// <summary>
        /// 更改Task
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("TaskState")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult Post(ReqModifyTaskState req)
        {
            var response = new ResViewTask();
            var userRepoModel = this.GetUserModelFromCurrentClaimsIdentity();
            if (!this._taskRepository.ModifyTaskState(req.taskID, req.teamID,
               req.state, userRepoModel.mail))
            {
                Enforce.Throw(new LogicErrorException("更改失败"));
            }
        
                return Get(new ReqViewTask() { taskID = req.taskID });  
        }

        /// <summary>
        /// Task详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult Get([FromUri]ReqViewTask req)
        {
            var response = new ResViewTask();
            var taskInfoModel = this._taskRepository.getTaskInfo(req.taskID);
            if (taskInfoModel == null)
            {
                Enforce.Throw(new LogicErrorException("不存在当前task"));
            }
            response.setResponse(ResStatusCode.OK, taskInfoModel, 1);
            return Ok(response);
        }

        [Route("TaskLogList")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult GetTaskLogList([FromUri]ReqViewTask req)
        {
            var response = new ResListTaskLog();
            var taskLoglist = this._taskRepository.getTaskLog(req.taskID);

            response.setResponse(ResStatusCode.OK, taskLoglist, taskLoglist.Count);
            return Ok(response);
        }
    }
}
