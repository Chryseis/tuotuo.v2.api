using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    public class BaseController : ApiController
    {
        protected IdentityService _identityService;
        public BaseController(IdentityService identityService)
        {
            _identityService = identityService;
        }
        public UserClaimsInfoModel GetUserModelFromCurrentClaimsIdentity()
        {
            UserClaimsInfoModel userClaimsInfoModel = null;
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                userClaimsInfoModel = _identityService.CreateUserClaimsInfoModelFromClaimsIdentity((ClaimsIdentity)principal.Identity);
            }

            return userClaimsInfoModel;

        }
    }
}
