using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public static class Enforce
    {
        /// <summary>
        /// 判断参数是否为null的参数，如果为null，则抛出ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="argument">判断是否为空的参数</param>
        /// <param name="description">异常的信息</param>
        /// <returns></returns>
        public static T ArgumentNotNull<T>(T argument, string description)
            where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(description);

            return argument;
        }

        /// <summary>
        /// 判断参数是否为null的参数，如果为null，则抛出需要的异常
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="argument">判断是否为空的参数</param>
        /// <param name="exception">需要抛出的异常</param>
        /// <returns></returns>
        public static T ArgumentNotNull<T>(T argument, Exception exception)
                where T : class
        {
            if (argument == null)
                throw exception;

            return argument;
        }

        /// <summary>
        /// 判断条件是否为真,如果为false，则抛出ArgumentException异常
        /// </summary>
        /// <param name="condition">需要判断的条件</param>
        /// <param name="message">异常的信息</param>
        public static void That(bool condition, string message)
        {
            if (condition == false)
            {
                throw new ArgumentException(message);
            }
        }


        /// <summary>
        /// 判断条件是否为真,如果为false，则抛出需要的异常
        /// </summary>
        /// <param name="condition">需要判断的条件</param>
        /// <param name="exception">需要抛出的异常</param>
        public static void That(bool condition, Exception exception)
        {
            if (condition == false)
            {
                throw exception;
            }
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="exception">需要抛出的异常</param>
        public static void Throw(Exception exception)
        {
            throw exception;
        }

    }
}
