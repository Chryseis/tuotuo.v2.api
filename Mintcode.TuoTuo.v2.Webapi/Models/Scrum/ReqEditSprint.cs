using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models
{
    public class ReqEditSprint : RequestBaseModel
    {
        public int sprintID { set; get; }

        public string startTime { set; get; }

        public string endTime { set; get; }
    }
}
