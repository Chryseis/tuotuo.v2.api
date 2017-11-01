using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public class Encrypt
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="soure"></param>
        /// <returns></returns>
        public static string Base64Encode(string soure)
        {
            byte[] enbuff = Encoding.UTF8.GetBytes(soure);
            return Convert.ToBase64String(enbuff);
        }
    }
}
