using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class TeamInfoModel
    {
        public int teamID { set; get; }
        public string teamName { set; get; }

        public string teamSummary { set; get; }

        public string teamAvatar { set; get; }

        public string createUserMail { set; get; }

        public DateTime createTime { set; get; }

        public string roleCode { get; set; }

        public long createTimestamp { get; set; }
    }
}
