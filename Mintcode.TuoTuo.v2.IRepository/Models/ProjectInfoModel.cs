using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class ProjectInfoModel
    {
        public int projectID { set; get; }
        public string projectName { set; get; }

        public string projectSummary { set; get; }

        public string projectAvatar { set; get; }

        public string createUserMail { set; get; }

        public DateTime createTime { set; get; }

        public string roleCode { get; set; }

        public long createTimestamp { set; get; }
    }
}
