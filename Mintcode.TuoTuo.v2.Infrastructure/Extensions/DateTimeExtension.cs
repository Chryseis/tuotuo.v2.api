using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 转化为时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.Now;
            long seconds = Convert.ToInt64((now - dateTime).TotalMilliseconds);

            long NowSeconds = Convert.ToInt64((now.ToUniversalTime() - epoch).TotalMilliseconds);
            return NowSeconds - seconds;
        }


        /// <summary>
        /// 根据日期获取日期是星期几
        /// </summary>
        /// <param name="text">日期</param>
        /// <returns>中文</returns>
        public static string MGetWeekDay(this DateTime p_DateTime)
        {
            string weekDay = "未知日期";

            switch (p_DateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    {
                        weekDay = "星期天";
                        break;
                    }
                case DayOfWeek.Monday:
                    {
                        weekDay = "星期一";
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        weekDay = "星期二";
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        weekDay = "星期三";
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        weekDay = "星期四";
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        weekDay = "星期五";
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        weekDay = "星期六";
                        break;
                    }
            }


            return weekDay;
        }

        /// <summary>
        /// 获取日期所属周的周一的日期
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static DateTime MGetWeekStart(this DateTime Date)
        {
            int weeknow = Convert.ToInt32(Date.DayOfWeek);
            int daydiff = (-1) * weeknow;
            int dayadd = 6 - weeknow;
            return DateTime.Parse(Date.AddDays(daydiff + 1).ToShortDateString());
        }

        /// <summary>
        /// 获取日期所属月的1号的日期
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static DateTime MGetMonthStart(this DateTime Date)
        {
            return DateTime.Parse(Date.AddDays(-(Date.Day) + 1).ToShortDateString());
        }
    }
}
