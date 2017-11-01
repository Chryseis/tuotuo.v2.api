using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.Scrum
{
    public class ReqDeleteSprint : RequestBaseModel
    {
        public int sprintID { set; get; }
    }
}
