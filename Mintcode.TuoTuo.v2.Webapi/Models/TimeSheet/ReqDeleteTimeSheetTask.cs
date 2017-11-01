using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet
{
    public class ReqDeleteTimeSheetTask:RequestBaseModel
    {
        public int taskID { set; get; }
    }
}
