using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.TuoTuo.v2.Repository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Models;
using Mintcode.TuoTuo.v2.Webapi.Models.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    [RoutePrefix("project")]
    public class ProjectController : BaseController
    {
        private IProjectRepository _projectRepo;
        public ProjectController(IProjectRepository projectRepo,  IdentityService identityService)
            : base(identityService)
        {
            _projectRepo = projectRepo;
        }

        [Route("GetMyProjects")]
        [HttpGet]
        [CustomAuthorize]
        public IHttpActionResult GetMyProjects()
        {
            var userRepoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var myProjects = _projectRepo.LoadProjectsByUserID(userRepoModel.userID);
            ResMyProjectList res = new ResMyProjectList();
            res.setResponse(ResStatusCode.OK, myProjects, myProjects.Count);
            return Ok(res);
        }

        /// <summary>
        /// 创建项目
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<IHttpActionResult> Put(ReqCreateProject req)
        {
            var response = new ResCreateProject();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var projectInfoModel = await _projectRepo.CreateProject(req.projectName,req.projectSummary, req.avatarToken,userClaimsInfoModel.mail);
            if (projectInfoModel.projectID<=0)
            {
                Enforce.Throw(new LogicErrorException("项目创建失败"));
            }          
            response.setResponse(ResStatusCode.OK, projectInfoModel,1);
            return Ok(response);
        }


        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult Get([FromUri]ReqViewProject req)
        {
            var response = new ResViewProject();
            var project = this._projectRepo.GetProjectDetail(req.projectID);
            response.setResponse(ResStatusCode.OK,project,1);
            return Ok(response);
        }

        /// <summary>
        /// 项目封面
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ViewProjectAvatar")]
        [HttpGet]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult ViewProjectAvatar([FromUri]ReqViewProjectAvatar req)
        {
            if (req.width<=0)
            {
                req.width = 280;
            }
            if (req.height <= 0)
            {
                req.height = 168;
            }
            var ms=this._projectRepo.GetProjectAvatar(req.projectID,req.avatar, req.width,req.height);               
            string contentType = AttachmentHelper.TransformExtensionToContentType(Path.GetExtension(req.avatar));
            return new FileResult(ms, contentType);
        }


        /// <summary>
        /// 更新项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner, RoleCode.Manager)]
        public async Task<IHttpActionResult> Post(ReqModifyProject req)
        {
            var response = new ResViewProject();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result = await _projectRepo.ModifyProject(req.projectID,req.projectName, req.projectSummary, req.avatarToken, userClaimsInfoModel.mail);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("项目更新失败"));
            }
            var project = this._projectRepo.GetProjectDetail(req.projectID);
            response.setResponse(ResStatusCode.OK, project, 1);
            return Ok(response);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("")]
        [HttpDelete]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner)]
        public IHttpActionResult Delete(ReqDeleteProject req)
        {
            var response = new ResponseBaseModel();
            var result = _projectRepo.DeleteProject(req.projectID);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("项目删除失败"));               
            }        
            response.SetResponse(ResStatusCode.OK, "项目删除成功");
            return Ok(response);
        }


        /// <summary>
        /// 邀请成员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("InviteMembers")]
        [HttpPost]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult InviteMembers(ReqInviteMember req)
        {
            var response = new ResInviteProject();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result=this._projectRepo.InviteMembers(req.projectID, userClaimsInfoModel.mail,req.mails);
            response.setResponse(ResStatusCode.OK,result,result.Count);
            return Ok(response);
        }


        /// <summary>
        /// 退出项目
        /// </summary>
        /// <returns></returns>
        [Route("ExitProject")]
        [HttpPost]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner, RoleCode.Manager, RoleCode.Member)]
        public IHttpActionResult ExitProject(ReqExitProject req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            this._projectRepo.ExitProject(req.projectID,userClaimsInfoModel.mail);
            response.SetResponse(ResStatusCode.OK, "退出项目成功");
            return Ok(response);
        }

        /// <summary>
        /// 转交项目
        /// </summary>
        /// <returns></returns>
        [Route("TransformProject")]
        [HttpPost]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner)]
        public IHttpActionResult TransformProject(ReqTransformProject req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result = this._projectRepo.TransformProject(req.projectID, userClaimsInfoModel.mail, req.mail);
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
        [CustomAuthorize(RoleType.Project, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult RemoveMember(ReqRemoveMember req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            this._projectRepo.RemoveProjectMember(req.projectID, req.mail, userClaimsInfoModel.mail);
            response.SetResponse(ResStatusCode.OK, "移除成功");
            return Ok(response);
        }


        /// <summary>
        /// 修改项目用户角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("ChangeMemberRole")]
        [HttpPost]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner)]
        public IHttpActionResult ChangeMemberRole(ReqChangeMemberRole req)
        {
            var response = new ResponseBaseModel();
            var result = this._projectRepo.ChangeMemberRole(req.projectID,req.mail,req.roleCode);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("修改失败"));
            }
            response.SetResponse(ResStatusCode.OK, "修改成功");
            return Ok(response);
        }


        /// <summary>
        /// 修改项目用户标签
        /// </summary>
        /// <returns></returns>
        [Route("ChangeMemberTags")]
        [HttpPost]
        [CustomAuthorize(RoleType.Project, RoleCode.Owner, RoleCode.Manager)]
        public IHttpActionResult ChangeMemberTags(ReqChangeMemberTags req)
        {
            var response = new ResponseBaseModel();
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();
            var result = this._projectRepo.ChangeMemberTags(req.projectID,userClaimsInfoModel.mail,req.mail,req.tags);
            if (!result)
            {
                Enforce.Throw(new LogicErrorException("修改失败"));
            }
            response.SetResponse(ResStatusCode.OK, "修改成功");
            return Ok(response);
        }


        /// <summary>
        /// 根据用户邮箱获取项目列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("GetProjectListByMail")]
        [HttpGet]
        [CustomAuthorize]
        public IHttpActionResult GetProjectListByMail([FromUri]ReqSearchProjectByMail req)
        {
            //Todo:(权限暂定)
            var myProjects = _projectRepo.LoadProjectsByUserMail(req.mail);
            ResMyProjectList res = new ResMyProjectList();
            res.setResponse(ResStatusCode.OK, myProjects, myProjects.Count);
            return Ok(res);
        }




    }
}
