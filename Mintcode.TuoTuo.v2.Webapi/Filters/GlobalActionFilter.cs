using Autofac.Integration.WebApi;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class GlobalActionFilter : IAutofacActionFilter
    {
        private IUserRepository userRepository;
        private IdentityService identityService;

        public GlobalActionFilter(IUserRepository _userRepository, IdentityService _identityService)
        {
            userRepository = _userRepository;
            identityService = _identityService;
        }
        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var actionAttrs = actionContext.ActionDescriptor.GetCustomAttributes<CustomAuthorizeAttribute>();

            if (actionAttrs.Count > 0)
            {
                var responseBaseModel = new ResponseBaseModel();

                ClaimsPrincipal principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
                if (principal != null && principal.Identity.IsAuthenticated)
                {
                    //认证成功         
                    if (actionAttrs.Any(s => null != s.Roles && s.Roles.Count() > 0))
                    {
                        //需要验证权限的情况 
                        List<Role> needRoles = actionAttrs
                            .Select(s => s.Roles.Select(m => new Role() { roleType = s.RoleType, roleCode = m }).ToList())
                            .Aggregate((result, next) =>
                            {
                                result.AddRange(next.Where(s => !result.Exists(m => m.roleType.Equals(s.roleType) && m.roleCode.Equals(s.roleCode))));
                                return result;
                            });
                        //当前用户具有的权限
                        //var oAuthRoles = principal.Claims.Where(s => s.Type.Equals(ClaimTypes.Role)).Select(s => s.Value).ToList();
                        //List<Role> localRoles = identityService.TransformOAuthRoleToLocalRole(oAuthRoles);
                        var userMail = ((ClaimsIdentity)principal.Identity).FindFirst(ClaimTypes.Email).Value;
                        var userRepoModel=this.userRepository.GetUser(userMail);
                        List<Role> localRoles = userRepoModel.roleList;
                        var reqParams = actionContext.ActionArguments.Values.FirstOrDefault() as RequestBaseModel;
                        if (needRoles.Count(s => s.roleType.Equals(RoleType.Team)).Equals(needRoles.Count()) && (null == reqParams || reqParams.teamID == 0))
                        {
                            //为了可读性，因此不采用!needRoles.Exists(s=>!s.roleType.Equals(RoleType.Team))
                            //全部是团队的角色
                            responseBaseModel.SetResponse(ResStatusCode.FrontInputValidateError, "团队ID不能为空");
                            SetOAuthErrorResponse(actionContext, responseBaseModel);
                        }
                        else if (needRoles.Count(s => s.roleType.Equals(RoleType.Project)).Equals(needRoles.Count()) && (null == reqParams || reqParams.projectID == 0))
                        {
                            //全部是项目的角色
                            responseBaseModel.SetResponse(ResStatusCode.FrontInputValidateError, "项目ID不能为空");
                            SetOAuthErrorResponse(actionContext, responseBaseModel);
                        }
                        else if (null == reqParams || (reqParams.teamID == 0 && reqParams.projectID == 0))
                        {
                            //混合项目和团队的角色(一般不可能发生)
                            responseBaseModel.SetResponse(ResStatusCode.FrontInputValidateError, "团队ID和项目ID不能同时为空");
                            SetOAuthErrorResponse(actionContext, responseBaseModel);
                        }
                        else if (!CheckAuthorize(localRoles, needRoles, reqParams))
                        {

                            //无角色匹配
                            responseBaseModel.SetResponse(ResStatusCode.UnAuthorize, "未授权用户");
                            SetOAuthErrorResponse(actionContext, responseBaseModel);
                        }

                    }
                }
                else
                {
                    //认证失败
                    responseBaseModel.SetResponse(ResStatusCode.UnAuthenticate, "未认证用户");
                    SetOAuthErrorResponse(actionContext, responseBaseModel);
                }

                #region 废弃代码

                //var response = new HttpResponseMessage();
                //var responseContent = new ResponseBaseModel();
                //var resMsg = new List<string>();
                //var reqParams = (RequestBaseModel)actionContext.ActionArguments.Values.FirstOrDefault();
                //reqParams = reqParams == null ? new RequestBaseModel() : reqParams;
                //if (string.IsNullOrEmpty(reqParams.createUser))
                //{
                //    resMsg.Add("创建人未填");
                //}
                //if (string.IsNullOrEmpty(reqParams.createUserName))
                //{
                //    resMsg.Add("创建人姓名未填");
                //}
                //if (reqParams.createTime < 0)
                //{
                //    resMsg.Add("创建时间未填");
                //}
                //if (string.IsNullOrEmpty(reqParams.token))
                //{
                //    resMsg.Add("token未填");
                //}

                //if (resMsg.Count > 0)
                //{
                //    responseContent.SetResponse(ResStatusCode.UnAuthenticate, String.Join(",", resMsg));
                //    response.Content = new StringContent(JsonConvert.SerializeObject(responseContent));
                //    actionContext.Response = response;
                //    actionContext.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //}

                #endregion

            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// 验证角色是否匹配
        /// </summary>
        /// <param name="localRoles"></param>
        /// <param name="needRoles"></param>
        /// <param name="reqParams"></param>
        /// <returns></returns>
        public bool CheckAuthorize(List<Role> localRoles, List<Role> needRoles, RequestBaseModel reqParams)
        {

            //团队角色的验证
            Predicate<Role> teamRoleMatch = s => s.roleType.Equals(RoleType.Team)
                                                && s.relationID.Equals(reqParams.teamID)
                                                && needRoles.Exists(m => m.roleCode.Equals(s.roleCode) && m.roleType.Equals(RoleType.Team));

            //项目角色的验证
            Predicate<Role> projectRoleMatch = s => s.roleType.Equals(RoleType.Project)
                                                && s.relationID.Equals(reqParams.projectID)
                                                && needRoles.Exists(m => m.roleCode.Equals(s.roleCode) && m.roleType.Equals(RoleType.Project));

            return localRoles.Exists(teamRoleMatch) || localRoles.Exists(projectRoleMatch);

        }

        public void SetOAuthErrorResponse(HttpActionContext actionContext, ResponseBaseModel content)
        {
            actionContext.Response = new HttpResponseMessage();
            actionContext.Response.Content = new StringContent(JsonConvert.SerializeObject(content));
            actionContext.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
