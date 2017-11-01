using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.Zeus.Public.Data
{
    public class LogHelper
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LogHelper));

        public static void Info(string msg)
        {
            log.Info(msg);
        }
    }
}
