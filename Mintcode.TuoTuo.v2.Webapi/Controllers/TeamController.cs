using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.TuoTuo.v2.Repository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Models;
using Mintcode.TuoTuo.v2.Webapi.Models.Team;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    [RoutePrefix("team")]
    public class TeamController : BaseController
    {
        private ITeamRepository _teamRepo;
        public TeamController(ITeamRepository teamRepo, IdentityService identityService)
            : base(identityService)
        {
            _teamRepo = teamRepo;
        }

        [Route("GetMyTeams")]
        [HttpGet]
        [CustomAuthorize]
        public IHttpActionResult GetMyTeams()
        {
            var userRepoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var myTeams = _teamRepo.LoadTeamsByUserID(userRepoModel.userID);
            ResMyTeamList res = new ResMyTeamList();
            res.setResponse(ResStatusCode.OK, myTeams, myTeams.Count);
            return Ok(res);
        }


        /// <summary>
        /// 创建团队
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<IHttpActionResult> Put(ReqCreateTeam req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            int id = await _teamRepo.CreateTeam(req.teamName, req.teamSummary, req.avatarToken, userClaimsInfoModel.mail);
            if (id == 0)
            {
                Enforce.Throw(new LogicErrorException("团队创建失败"));
            }
            return Get(new ReqViewTeam() { teamID = id });
        }


        /// <summary>
        /// 获取团队详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult Get([FromUri]ReqViewTeam req)
        {
            var response = new ResViewTeam();
            var team = this._teamRepo.GetTeamDetail(req.teamID);
            response.setResponse(ResStatusCode.OK, team, 1);
            return Ok(response);
        }

        /// <summary>
        /// 更新团队
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public async Task<IHttpActionResult> Post(ReqModifyTeam req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result = await _teamRepo.ModifyTeam(req.teamID, req.teamName, req.teamSummary, req.avatarToken, userClaimsInfoModel.mail);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("团队更新失败"));
            }
            // response.SetResponse(ResStatusCode.OK, "团队更新成功");
            // return Ok(response);
            return Get(new ReqViewTeam() { teamID = req.teamID });
        }

        /// <summary>
        /// 删除团队
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpDelete]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner)]
        public IHttpActionResult Delete(ReqDeleteTeam req)
        {
            var response = new ResponseBaseModel();
            var result = _teamRepo.DeleteTeam(req.teamID);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("团队删除失败"));
            }
            response.SetResponse(ResStatusCode.OK, "团队删除成功");
            return Ok(response);
        }


        /// <summary>
        /// 邀请成员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("InviteMembers")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult InviteMembers(ReqInviteMember req)
        {
            var response = new ResInviteTeam();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result = this._teamRepo.InviteMembers(req.teamID, userClaimsInfoModel.mail, req.mails);
            response.setResponse(ResStatusCode.OK, result, result.Count);
            return Ok(response);
        }


        /// <summary>
        /// 退出团队
        /// </summary>
        /// <returns></returns>
        [Route("ExitTeam")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult ExitTeam(ReqExitTeam req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            this._teamRepo.ExitTeam(req.teamID, userClaimsInfoModel.mail);
            response.SetResponse(ResStatusCode.OK, "退出团队成功");
            return Ok(response);
        }

        /// <summary>
        /// 转交团队
        /// </summary>
        /// <returns></returns>
        [Route("TransformTeam")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner)]
        public IHttpActionResult TransformTeam(ReqTransformTeam req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result = this._teamRepo.TransformTeam(req.teamID, userClaimsInfoModel.mail, req.mail);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("转交失败"));
            }
            response.SetResponse(ResStatusCode.OK, "转交成功");
            return Ok(response);
        }


        /// <summary>
        /// 移除成员
        /// </summary>
        /// <returns></returns>
        [Route("RemoveMember")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult RemoveMember(ReqRemoveMember req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            this._teamRepo.RemoveTeamMember(req.teamID, req.mail, userClaimsInfoModel.mail);
            response.SetResponse(ResStatusCode.OK, "移除成功");
            return Ok(response);
        }

        [Route("MemberList")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult MemberList(ReqViewTeam req)
        {
            var response = new ResMyTeamMemberList();

            var memberList = this._teamRepo.getTeamMemberList(req.teamID);
            response.setResponse(ResStatusCode.OK, memberList, memberList.Count);
            return Ok(response);
        }


        /// <summary>
        /// 修改团队用户角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ChangeMemberRole")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner)]
        public IHttpActionResult ChangeMemberRole(ReqChangeMemberRole req)
        {
            var response = new ResponseBaseModel();
            var result = this._teamRepo.ChangeMemberRole(req.teamID, req.mail, req.roleCode);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("修改失败"));
            }
            response.SetResponse(ResStatusCode.OK, "修改成功");
            return Ok(response);
        }


        /// <summary>
        /// 修改团队用户标签
        /// </summary>
        /// <returns></returns>
        [Route("ChangeMemberTags")]
        [HttpPost]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult ChangeMemberTags(ReqChangeMemberTags req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result = this._teamRepo.ChangeMemberTags(req.teamID, userClaimsInfoModel.mail, req.mail, req.tags);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("修改失败"));
            }
            response.SetResponse(ResStatusCode.OK, "修改成功");
            return Ok(response);
        }
        [Route("SearchTeam")]
        [HttpPost]
        [CustomAuthorize]
        public IHttpActionResult SearchTeam(ReqSearchTeam req)
        {
            var response = new ResSearchTeam();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result = this._teamRepo.searchTeamAndMember(userClaimsInfoModel.userID, req.searchName);
            response.setResponse(ResStatusCode.OK, result, 1);
            return Ok(response);
        }

        /// <summary>
        /// 团队封面
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ViewTeamAvatar")]
        [HttpGet]
        [CustomAuthorize(RoleType.Team, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult ViewTeamAvatar([FromUri]ReqViewTeamAvatar req)
        {
            if (req.width <= 0)
            {
                req.width = 280;
            }
            if (req.height <= 0)
            {
                req.height = 168;
            }
            var ms = this._teamRepo.GetTeamAvatar(req.teamID, req.avatar, req.width, req.height);
            string contentType = AttachmentHelper.TransformExtensionToContentType(Path.GetExtension(req.avatar));
            return new FileResult(ms, contentType);
        }

    }
}
