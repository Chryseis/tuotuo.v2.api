using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public static class DoubleExtension
    {
        /// <summary>
        /// 两数相除
        /// </summary>
        /// <param name="divisor">除数</param>
        /// <param name="dividend">被除数</param>
        /// <param name="defaultValue">当被除数为0时，返回的默认值</param>
        /// <param name="stringFormat">
        /// 返回结果的格式，默认N2
        /// 同String.Format格式,N2=小数点后两位，p=百分比...
        /// </param>
        /// <returns></returns>
        public static string MDeduct(this double divisor, double dividend, string defaultValue, string stringFormat)
        {
            string retValue = defaultValue;
            if (string.IsNullOrEmpty(stringFormat))
            {
                stringFormat = "N2";
            }
            if (dividend != 0)
            {
                retValue = string.Format("{0:" + stringFormat + "}", divisor / dividend);
            }
            return retValue;
        }
    }
}
