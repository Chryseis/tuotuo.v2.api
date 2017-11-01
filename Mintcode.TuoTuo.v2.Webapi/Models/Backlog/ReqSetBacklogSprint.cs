using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models
{
    public class ReqSetBacklogSprint:RequestBaseModel
    {
        public int sprintID { set; get; }

        public List<int> backlogIDs { set; get; }
    }
}
