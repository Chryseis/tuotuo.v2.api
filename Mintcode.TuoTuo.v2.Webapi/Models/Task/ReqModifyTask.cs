using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.Task
{
    public class ReqModifyTask:RequestBaseModel
    {
        public int taskID { get; set; }
        public string title { get; set; }

        public string content { get; set; }

        public string assignedEmail { get; set; }

        public string typeName { get; set; }

        public int time { get; set; }

        public int state { get; set; }
    }
}
