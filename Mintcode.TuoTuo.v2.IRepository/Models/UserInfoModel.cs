using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    /// <summary>
    /// 单纯的用户信息(不包含角色)
    /// </summary>
    public class UserInfoModel
    {
        public int userID { set; get; }
        public string userName { get; set; }

        public string userTrueName { get; set; }

        public int userLevel { get; set; }

        public string password { set; get; }

        public string mail { get; set; }

        public string mobile { set; get; }

        public string userAvatar { set; get; }

        public int userStatus { get; set; }

        public int? sex { set; get; }

        public DateTime lastLoginTime { set; get; }

        public DateTime createTime { set; get; }


        public long lastLoginTimestamp { set; get; }

        public long createTimestamp { set; get; }
    }
}
