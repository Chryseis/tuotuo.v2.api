using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    /// <summary>
    /// String类的扩展
    /// </summary>
    public static partial class StringExtension
    {
        #region 截取字符串

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="length">需要截取的字符串长度</param>
        /// <returns></returns>
        public static string MGetString(this string sourceString, int length)
        {
            return MGetString(sourceString, length, string.Empty);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="length">需要截取的字符串长度</param>
        /// <param name="suffix">
        /// 后缀,截取完字符串后加在字符串最后,如...
        /// 只有当源字符串长度大于需要截取的长度时候才会添加</param>
        /// <returns></returns>
        public static string MGetString(this string sourceString, int length, string suffix)
        {
            int subLength = length;
            if (!string.IsNullOrEmpty(sourceString) && sourceString.Length > subLength)
            {
                sourceString = sourceString.Substring(0, subLength);
                sourceString += suffix;
            }
            return sourceString;
        }

        /// <summary>
        /// 截取字符串(字符串包含中文)
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="length">需要截取的字符串长度</param>
        /// <returns></returns>
        public static string MGetStringCN(this string sourceString, int length)
        {
            return MGetStringCN(sourceString, length, string.Empty);
        }

        /// <summary>
        /// 截取字符串(字符串包含中文)
        /// </summary>
        /// <param name="sourceString">包含中文的文本</param>
        /// <param name="len">最长长度值</param>
        /// <param name="suffix">后缀,截取完字符串后加在字符串最后,如...
        /// 只有当源字符串长度大于需要截取的长度时候才会添加</param>
        /// <returns></returns>
        public static string MGetStringCN(this string sourceString, int length, string suffix)
        {
            sourceString = Regex.Replace(sourceString, "<[^>]*>", "");
            int byteLen = System.Text.Encoding.Default.GetByteCount(sourceString);  //单字节字符长度
            int charLen = sourceString.Length; //把字符平等对待时的字符串长度
            int byteCount = 0;  //记录读取进度{中文按两单位计算}
            int pos = 0;    //记录截取位置{中文按两单位计算}
            char[] sourceArray = sourceString.ToCharArray();
            int subLength = length + suffix.Length;
            if (byteLen > subLength)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(sourceArray[i]) > 255)  //遇中文字符计数加2
                        byteCount += 2;
                    else         //按英文字符计算加1
                        byteCount += 1;
                    if (byteCount >= subLength)   //到达指定长度时，记录指针位置并停止
                    {
                        pos = i;
                        break;
                    }
                }
                sourceString = sourceString.Substring(0, pos) + suffix;
            }
            return sourceString;
        }

        /// <summary>
        /// 删除不可见字符
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string MDeleteUnVisibleChar(this string sourceString)
        {
            StringBuilder sBuilder = new StringBuilder(131);
            for (int i = 0; i < sourceString.Length; i++)
            {
                int Unicode = sourceString[i];
                if (Unicode >= 16)
                {
                    sBuilder.Append(sourceString[i].ToString());
                }
            }
            return sBuilder.ToString();
        }

        #endregion

        #region Html

        /// <summary>
        /// 过滤html
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string MFilterHtml(this string html)
        {
            Regex regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" on[\s\S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex6 = new Regex(@"\<img[^\>]+\>", RegexOptions.IgnoreCase);
            Regex regex7 = new Regex(@"</p>", RegexOptions.IgnoreCase);
            Regex regex8 = new Regex(@"<p>", RegexOptions.IgnoreCase);
            Regex regex9 = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            Regex regex10 = new Regex(@"<style[\s\S]+</style *>", RegexOptions.IgnoreCase);
            html = regex10.Replace(html, "");//先过滤<style></style>标记
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");

            //html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }

        #endregion

        /// <summary>
        /// 去除最后一个分隔符（如果存在的话,默认逗号）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MDeleteLastSparator(this string input)
        {
            return input.MDeleteLastSparator(",");
        }

        /// <summary>
        /// 去除最后一个分隔符（如果存在的话）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sparator"></param>
        /// <returns></returns>
        public static string MDeleteLastSparator(this string input, string sparator)
        {
            if (!string.IsNullOrEmpty(input) && input.EndsWith(sparator))
            {
                input = input.Substring(0, input.Length - sparator.Length);
            }
            return input;
        }
    }
}
