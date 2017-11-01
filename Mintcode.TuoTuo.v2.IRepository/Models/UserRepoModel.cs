using Mintcode.TuoTuo.v2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class UserRepoModel
    {
        
        public UserInfoModel info { set; get; }

        public List<Role> roleList { get; set; }

    }

    public class Role
    {
        public RoleType roleType { get; set; }

        public int relationID { get; set; }

        public string roleCode { get; set; }
    }
}
