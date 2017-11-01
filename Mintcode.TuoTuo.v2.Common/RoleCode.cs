using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Common
{
    public class RoleCode
    {
        //拥有者
        public  const string Owner="OWNER";

        //管理者
        public const string Manager = "MANAGER";

        //成员
        public const string Member = "MEMBER";


        public static bool CheckCode(string code)
        {
            bool result = false;
            switch (code)
            {
                case RoleCode.Owner:
                case RoleCode.Manager:
                case RoleCode.Member:
                    result = true;
                    break;
                default:result = false;break;
            }
            return result;
        }

    }
}
