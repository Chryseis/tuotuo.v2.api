using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Common
{
    public enum ResStatusCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        OK = 8200,

        /// <summary>
        /// 内部逻辑正常错误
        /// </summary>
        LogicError=8300,

        /// <summary>
        /// 用户输入参数验证失败
        /// </summary>
        UserInputValidateError = 8400,

        /// <summary>
        /// 未验证
        /// </summary>
        UnAuthenticate = 8401,

        /// <summary>
        /// 未授权
        /// </summary>
        UnAuthorize = 8402,

        /// <summary>
        /// 前端App输入参数验证失败
        /// </summary>
        FrontInputValidateError = 8403,

        /// <summary>
        /// 内部错误
        /// </summary>
        InternalServerError = 8500


    }
}
