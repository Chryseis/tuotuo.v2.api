using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public static class ObjectExtensions
    {

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="inputValue">数据</param>
        /// <returns></returns>
        public static T MConvertTo<T>(this object inputValue)
            where T : IConvertible
        {
            return inputValue.MConvertTo<T>(default(T));
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="inputValue">数据</param>
        /// <param name="defaultValue">转换失败时的默认值</param>
        /// <returns></returns>
        public static T MConvertTo<T>(this object inputValue, T defaultValue)
             where T : IConvertible
        {
            T retValue = defaultValue;
            try
            {
                Type conversionType = typeof(T);
                //是否可空类型
                if (inputValue != null)
                {
                    if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        NullableConverter nullableConverter = new NullableConverter(conversionType);
                        conversionType = nullableConverter.UnderlyingType;
                        retValue = (T)Convert.ChangeType(inputValue, conversionType);
                    }
                    else
                    {
                        retValue = (T)Convert.ChangeType(inputValue, typeof(T));
                    }
                }
            }
            catch
            {
                retValue = defaultValue;
            }
            return retValue;
        }
    }
}
