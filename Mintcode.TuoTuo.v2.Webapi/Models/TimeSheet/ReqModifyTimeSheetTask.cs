using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet
{
    public class ReqModifyTimeSheetTask:RequestBaseModel
    {
        public int taskID { set; get; }

        public string detail { set; get; }

        public int selectProjectID { set; get; }

        public decimal time { set; get; }
    }
}
