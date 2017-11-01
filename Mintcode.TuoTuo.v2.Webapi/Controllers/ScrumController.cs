using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Models;
using Mintcode.TuoTuo.v2.Webapi.Models.Scrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    [RoutePrefix("scrum")]
    public class ScrumController : BaseController
    {
        private IScrumRepository _scrumRepository;
        public ScrumController(IScrumRepository scrumRepository,IdentityService identityService)
            : base(identityService)
        {
            this._scrumRepository = scrumRepository;
        }

        /// <summary>
        /// 创建Release
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("CreateRelease")]
        [HttpPut]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public  IHttpActionResult CreateRelease (ReqCreateRelease req)
        {
            var response = new ResCreateRelease();
            UserClaimsInfoModel userClaimsInfoModel=this.GetUserModelFromCurrentClaimsIdentity();
            var releaseInfoModel = this._scrumRepository.CreateRelease(req.teamID,req.releaseName, req.releaseSummary, userClaimsInfoModel.mail);
            if (releaseInfoModel.ID<=0)
            {
                Enforce.Throw(new LogicErrorException("Release创建失败"));
            }
            response.setResponse(ResStatusCode.OK, releaseInfoModel,1);
            return Ok(response);
        }


        /// <summary>
        /// Release详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ViewRelease")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult ViewRelease([FromUri]ReqViewRelease req)
        {
            var response = new ResViewRelease();
            var releaseInfoModel=this._scrumRepository.ViewRelease(req.teamID,req.releaseID);
            if (releaseInfoModel==null)
            {
                Enforce.Throw(new LogicErrorException("Release 不存在"));
            }
            response.setResponse(ResStatusCode.OK, releaseInfoModel, 1);
            return Ok(response);
        }


        /// <summary>
        /// Release 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ListRelease")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult ListRelease([FromUri]RequestBaseModel req)
        {
            var response = new ResListRelease();
            var releaseInfoModelList = this._scrumRepository.ListRelease(req.teamID);           
            response.setResponse(ResStatusCode.OK, releaseInfoModelList, releaseInfoModelList.Count);
            return Ok(response);
        }

        /// <summary>
        /// 删除Release
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("DeleteRelease")]
        [HttpDelete]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult DeleteRelease(ReqDeleteRelease req)
        {
            var response = new ResponseBaseModel();
            var ret = this._scrumRepository.DeleteRelease(req.teamID, req.releaseID);
            if (!ret)
            {
                Enforce.Throw(new LogicErrorException("Release删除失败"));
            }
            response.SetResponse(ResStatusCode.OK, "Release删除成功");
            return Ok(response);
        }



        /// <summary>
        /// 创建Sprint
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("CreateSprint")]
        [HttpPut]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult CreateSprint(ReqCreateSprint req)
        {
            var response = new ResCreateSprint();  
            UserClaimsInfoModel userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var sprintInfoModel = this._scrumRepository.CreateSprint(req.teamID, req.releaseID,
                Infrastructure.Util.DateTimeUtils.CreateDateTime(req.startTime),
                Infrastructure.Util.DateTimeUtils.CreateDateTime(req.endTime), userClaimsInfoModel.mail);
            if (sprintInfoModel.ID<=0)
            {
                Enforce.Throw(new LogicErrorException("Sprint创建失败"));
            }
            response.setResponse(ResStatusCode.OK, sprintInfoModel,1);

            return Ok(response);
        }


        /// <summary>
        /// Sprint 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ListSprint")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult ListSprint([FromUri]ReqListSprint req)
        {
            var response = new ResListSprint();
            var sprintInfoModelList = this._scrumRepository.ListSprint(req.teamID,req.releaseID);
            response.setResponse(ResStatusCode.OK, sprintInfoModelList, sprintInfoModelList.Count);
            return Ok(response);
        }


        /// <summary>
        /// 设置当前Sprint
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("SetCurrentSprint")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult SetCurrentSprint(ReqSetCurrentSprint req)
        {
            var response = new ResponseBaseModel();
            UserClaimsInfoModel userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var ret = this._scrumRepository.SetCurrentSprint(req.teamID, req.sprintID);
            if (!ret)
            {
                Enforce.Throw(new LogicErrorException("Sprint设置失败"));
            }
            response.SetResponse(ResStatusCode.OK, "Sprint设置成功");

            return Ok(response);
        }


        /// <summary>
        /// 编辑Sprint
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("EditSprint")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult EditSprint(ReqEditSprint req)
        {
            var response = new ResponseBaseModel();
            var ret = this._scrumRepository.EditSprint(req.teamID, req.sprintID,DateTime.Parse(req.startTime),DateTime.Parse(req.endTime));
            if (!ret)
            {
                Enforce.Throw(new LogicErrorException("Sprint修改失败"));
            }
            response.SetResponse(ResStatusCode.OK, "Sprint修改成功");

            return Ok(response);
        }


        /// <summary>
        /// 删除Sprint
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("DeleteSprint")]
        [HttpDelete]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult DeleteSprint(ReqDeleteSprint req)
        {
            var response = new ResponseBaseModel();
            this._scrumRepository.DeleteSprint(req.teamID, req.sprintID);
            response.SetResponse(ResStatusCode.OK, "Sprint删除成功");

            return Ok(response);
        }




        /// <summary>
        /// Release 和 Sprint的列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ReleaseAndSprintList")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult ReleaseAndSprintList([FromUri]RequestBaseModel req)
        {
            ResReleaseAndSprintList response = new ResReleaseAndSprintList();
            var list=this._scrumRepository.GetReleaseAndSprintList(req.teamID);
            response.setResponse(ResStatusCode.OK, list, list.Count);
            return Ok(response);
        }


        /// <summary>
        /// 获取当前团队的Release和Sprint
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("GetCurrentReleaseAndSprint")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult GetCurrentReleaseAndSprint([FromUri]RequestBaseModel req)
        {
            ResCurrentReleaseAndSprint response = new ResCurrentReleaseAndSprint();
            var currentReleaseAndSprint = this._scrumRepository.GetCurrentReleaseAndSprint(req.teamID);
            if (currentReleaseAndSprint == null)
            {
                Enforce.Throw(new LogicErrorException("未设置当前Sprint"));
            }
            response.setResponse(ResStatusCode.OK, currentReleaseAndSprint, 1);
            return Ok(response);
        }




    }
}
