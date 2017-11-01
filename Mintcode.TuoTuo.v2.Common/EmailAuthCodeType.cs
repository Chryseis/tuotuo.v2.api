using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Common
{
    public enum EmailAuthCodeType
    {
        //注册用户
        RegisterUser = 1,

        //绑定用户
        BindUser = 2,

        //重置密码
        ResetPassword = 3
    }
}
