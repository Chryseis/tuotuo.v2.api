using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class RequestBaseModel
    {
        ///// <summary>
        ///// 语言
        ///// </summary>
        //public string lang { get; set; }

        ///// <summary>
        ///// 创建人姓名
        ///// </summary>
        //public string createUserName { get; set; }

        ///// <summary>
        ///// 创建人userId
        ///// </summary>
        //public string createUser { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public long createTime { get; set; }

        ///// <summary>
        ///// 登陆令牌
        ///// </summary>
        //public string token { get; set; }

        /// <summary>
        /// 团队ID
        /// </summary>
        public int teamID { set; get; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int projectID { set; get; }
    }
}
