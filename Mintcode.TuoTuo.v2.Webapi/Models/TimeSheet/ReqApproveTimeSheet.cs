using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet
{
    public class ReqApproveTimeSheet : RequestBaseModel
    {
        public int sheetID { set; get; }

        public int result { set; get; }

        public string comment { set; get; }

        public long viewTimeStamp { set; get; }
    }
}
