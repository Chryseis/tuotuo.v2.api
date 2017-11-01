using Mintcode.TuoTuo.v2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Identity
{
    public class CustomAuthorizeAttribute : Attribute
    {
        public string[] Roles { get; set; }

        public RoleType RoleType { get; set; }

        public CustomAuthorizeAttribute()
        {

        }

        public CustomAuthorizeAttribute(RoleType roleType, params string[] roles)
        {
            RoleType = roleType;
            Roles = roles;
        }

    }
}
