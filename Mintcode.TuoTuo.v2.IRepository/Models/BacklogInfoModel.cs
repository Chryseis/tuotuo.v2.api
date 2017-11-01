using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class BacklogInfoModel
    {
        public int ID { set; get; }
        public int teamID { set; get; }

        public int projectID { set; get; }

        public string projectName { set; get; }

        public int sprintID { set; get; }

        public string title { set; get; }

        public string content { set; get; }

        public string standard { set; get; }

        public int state { set; get; }

        public int? level { set; get; }

        public string assignUserName { set; get; }

        public string assignUserMail { set; get; }

        public string createUserMail { set; get; }

        public DateTime createTime { set; get; }

        public long createTimestamp { set; get; }
    }
}
