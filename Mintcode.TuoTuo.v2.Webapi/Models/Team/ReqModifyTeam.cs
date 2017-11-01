using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.Team
{
    public class ReqModifyTeam : RequestBaseModel
    {
        public string teamName { set; get; }
        public string teamSummary { set; get; }
        public string avatarToken { set; get; }
    }
}
