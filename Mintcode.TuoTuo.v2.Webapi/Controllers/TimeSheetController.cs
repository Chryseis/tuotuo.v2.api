using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.Infrastructure.Util;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    [RoutePrefix("timesheet")]
    public class TimeSheetController : BaseController
    {
        private ITimeSheetRepository _timeSheetRepo;
        public TimeSheetController(ITimeSheetRepository timeSheetRepo,IdentityService identityService) 
            : base(identityService)
        {
            this._timeSheetRepo = timeSheetRepo;
        }

        /// <summary>
        /// 获取TimeSheet详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("GetMyTimeSheet")]
        [HttpGet]
        [CustomAuthorize]
        public IHttpActionResult GetMyTimeSheet([FromUri]ReqMyTimeSheet req)
        {
            DateTime date;
            if (req.currentTimeStamp <= 0)
            {
                 date = DateTime.Now;
            }
            else
            {
                 date=DateTimeUtils.CreateDateTime(req.currentTimeStamp);
            }
            req.currentTimeStamp = new DateTime(date.Year, date.Month, date.Day).ToTimeStamp();

            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            ResMyTimeSheet res = new ResMyTimeSheet();
            var timeSheetRepoModel=this._timeSheetRepo.GetTimeSheetDetail(currentInfo.mail,req.currentTimeStamp,false);
            if (timeSheetRepoModel==null)
            {
                //当前时间的TimeSheet不存在则创建   
                if (!this._timeSheetRepo.CreateTimeSheet(currentInfo.mail, req.currentTimeStamp))
                {
                    Enforce.Throw(new LogicErrorException(""));   
                }
                timeSheetRepoModel = this._timeSheetRepo.GetTimeSheetDetail(currentInfo.mail, req.currentTimeStamp, false);
            }
            res.setResponse(ResStatusCode.OK, timeSheetRepoModel,1);
            return Ok(res);
        }



        /// <summary>
        /// 创建TimeSheet Tasks
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("CreateTimeSheetTasks")]
        [HttpPost]
        [CustomAuthorize]
        public IHttpActionResult CreateTimeSheetTasks([FromBody]ReqCreateTimeSheetTasks req)
        {
            var response = new ResCreateTimeSheetTasks();
            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            var result=this._timeSheetRepo.CreateTimeSheetTasks(req.sheetID,req.tasks, currentInfo.mail);
            response.setResponse(ResStatusCode.OK, result, result.Count);
            return Ok(response);
        }

        /// <summary>
        /// 删除Time Sheet Task
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("DeleteTimeSheetTask")]
        [HttpDelete]
        [CustomAuthorize]
        public IHttpActionResult DeleteTimeSheetTask([FromBody]ReqDeleteTimeSheetTask req)
        {
            var response = new ResponseBaseModel();
            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            this._timeSheetRepo.DeleteTimeSheetTask(req.taskID, currentInfo.mail);
            response.SetResponse(ResStatusCode.OK,"删除成功");
            return Ok(response);
        }


        /// <summary>
        /// 更改Time Sheet Task
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ModifyTimeSheetTask")]
        [HttpPost]
        [CustomAuthorize]
        public IHttpActionResult ModifyTimeSheetTask([FromBody]ReqModifyTimeSheetTask req)
        {
            var response = new ResponseBaseModel();
            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            if (!this._timeSheetRepo.ModifyTimeSheetTask(req.taskID, req.detail, req.selectProjectID, req.time, currentInfo.mail))
            {
                Enforce.Throw(new LogicErrorException("更改失败"));
            }
            response.SetResponse(ResStatusCode.OK, "更改成功");
            return Ok(response);
        }




        /// <summary>
        /// 审核TimeSheet
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ApproveTimeSheet")]
        [HttpPost]
        [CustomAuthorize]
        public IHttpActionResult ApproveTimeSheet([FromBody]ReqApproveTimeSheet req)
        {
            var response = new ResponseBaseModel();
            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            var ret=this._timeSheetRepo.ApproveTimeSheet(req.sheetID, (TimeSheetResultStatus)req.result, req.comment, req.viewTimeStamp, currentInfo.mail);
            if (!ret)
            {
                Enforce.Throw(new LogicErrorException("审核失败"));
            }
            response.SetResponse(ResStatusCode.OK, "审核成功");
            return Ok(response);
        }


        /// <summary>
        /// 提交TimeSheet
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("SubmitTimeSheet")]
        [HttpPost]
        [CustomAuthorize]
        public IHttpActionResult SubmitTimeSheet([FromBody]ReqSubmitTimeSheet req)
        {
            var response = new ResponseBaseModel();
            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            var ret = this._timeSheetRepo.SubmitTimeSheet(req.sheetID,currentInfo.mail);
            if (!ret)
            {
                Enforce.Throw(new LogicErrorException("提交失败"));
            }
            response.SetResponse(ResStatusCode.OK, "提交成功");
            return Ok(response);
        }



        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <returns></returns>
        [Route("GetQueryParams")]
        [HttpGet]
        [CustomAuthorize]
        public IHttpActionResult GetQueryParams()
        {
            var res = new ResGetQueryParams();
            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            var queryParamModelList= this._timeSheetRepo.GetQueryParams(currentInfo.userID);
            res.setResponse(ResStatusCode.OK, queryParamModelList, queryParamModelList.Count);
            return Ok(res);
        }


        /// <summary>
        /// 审批的TimeSheet列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("QueryCheckTimeSheetList")]
        [HttpPost]
        [CustomAuthorize]
        public IHttpActionResult QueryCheckTimeSheetList([FromBody]ReqGetTimeSheetCheckList req)
        {
            var res = new ResGetTimeSheetCheckList();
            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            long total = 0;
            var list = this._timeSheetRepo.GetCheckTimeSheetRepoModelList(currentInfo.userID,req.startTime,req.endTime,req.selectTeamIDList,
                req.selectUserIDList,req.selectStatusList,req.from,req.to, out total);
            res.setResponse(ResStatusCode.OK,list, total);
            return Ok(res);
        }

        /// <summary>
        /// TimeSheet 报表列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("QueryReportTimeSheetList")]
        [HttpPost]
        [CustomAuthorize]
        public IHttpActionResult QueryReportTimeSheetList([FromBody]ReqQueryReportTimeSheetList req)
        {
            var res = new ResQueryReportTimeSheetList();
            var currentInfo = this.GetUserModelFromCurrentClaimsIdentity();
            long total = 0;
            var model=this._timeSheetRepo.GetReportTimeSheetList(currentInfo.userID, req.startTime, req.endTime, req.selectTeamIDList,
                req.selectUserIDList,req.selectProjectIDList,req.from,req.to,out total);
            res.setResponse(ResStatusCode.OK, model, total);
            return Ok(res);
        }
    }
}
