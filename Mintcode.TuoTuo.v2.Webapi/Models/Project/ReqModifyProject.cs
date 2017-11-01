using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models
{
    public class ReqModifyProject : RequestBaseModel
    {
        public string projectName { set; get; }
        public string projectSummary { set; get; }
        public string avatarToken { set; get; }
    }
}
