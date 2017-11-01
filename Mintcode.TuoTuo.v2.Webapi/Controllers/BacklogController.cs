using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Models;
using Mintcode.TuoTuo.v2.Webapi.Models.Backlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    [RoutePrefix("backlog")]
    public class BacklogController : BaseController
    {
        private IBacklogRepository _backlogRepository;
        public BacklogController(IBacklogRepository backlogRepository,IdentityService identityService)
            : base(identityService)
        {
            this._backlogRepository = backlogRepository;
        }


        /// <summary>
        /// Backlog 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ListBacklog")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult ListBacklog([FromUri]ReqListBacklog req)
        {
            var response = new ResListBacklog();
            var backInfoList=this._backlogRepository.GetBacklogInfoList(req.teamID, req.sprintID);
            response.setResponse(ResStatusCode.OK, backInfoList, backInfoList.Count);
            return Ok(response);
        }

        [Route("ListBacklogAndTask")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult ListBacklogAndTask([FromUri]ReqListBacklogAndTask req)
        {
            var response = new ResListBacklogAndTask();
            var backRepoList = this._backlogRepository.GetBacklogRepoList(req.teamID, req.sprintID);
            response.setResponse(ResStatusCode.OK, backRepoList, backRepoList.Count);
            return Ok(response);
        }


        /// <summary>
        ///设置Backlog的Sprint
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("SetBacklogSprint")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult SetBacklogSprint(ReqSetBacklogSprint req)
        {
            var response = new ResponseBaseModel();
            if (!this._backlogRepository.SetBackLogsSprint(req.teamID,req.sprintID,req.backlogIDs))
            {
                Enforce.Throw(new LogicErrorException("设置失败"));
            }
            response.SetResponse(ResStatusCode.OK,"设置成功");
            return Ok(response);
        }



        /// <summary>
        /// 创建Backlog
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult Put(ReqCreateBacklog req)
        {
            var userRepoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var response = new ResCreateBacklog();
            var backlogInfoModel = this._backlogRepository.CreateBackLog(req.teamID, req.title, req.content, req.standard,
                req.assignUserMail, req.selectProjectID, req.state, req.level, userRepoModel.mail);
            if (backlogInfoModel.ID<=0)
            {
                Enforce.Throw(new LogicErrorException("添加失败"));
            }
            response.setResponse(ResStatusCode.OK, backlogInfoModel,1);
            return Ok(response);
        }


        /// <summary>
        /// 更改Backlog
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult Post(ReqModifyBacklog req)
        {           
            var response = new ResViewBacklog();
            var modifyBackLogResult = this._backlogRepository.ModifyBackLog(req.teamID, req.backlogID, req.title, req.content, req.standard,
                req.assignUserMail, req.selectProjectID, req.state, req.level);
            if (modifyBackLogResult==null)
            {
                Enforce.Throw(new LogicErrorException("更改失败"));
            }
            response.setResponse(ResStatusCode.OK, modifyBackLogResult,1);
            return Ok(response);
        }



        /// <summary>
        /// Backlog详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult Get([FromUri]ReqViewBacklog req)
        {
            var response = new ResViewBacklog();
            var backlogInfoModel=this._backlogRepository.GetBacklogInfoModel(req.teamID,req.backlogID);
            if (backlogInfoModel==null)
            {
                Enforce.Throw(new LogicErrorException("不存在当前backlog"));
            }
            response.setResponse(ResStatusCode.OK,backlogInfoModel,1);
            return Ok(response);
        }


        /// <summary>
        /// 删除Backlog
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpDelete]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult Delete([FromBody]ReqDeleteBacklog req)
        {
            var response = new ResponseBaseModel();
            if (!this._backlogRepository.DeleteBacklog(req.teamID,req.backlogID))
            {
                Enforce.Throw(new LogicErrorException("删除失败"));
            }
            response.SetResponse(ResStatusCode.OK, "删除成功");
            return Ok(response);  
        }
    }
}
