using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class SprintInfoModel
    {
        public int ID { set; get; }

        public int releaseID { set; get; }

        public DateTime startTime { set; get; }

        public long startTimestamp { set; get; }

        public DateTime endTime { set; get; }

        public long endTimestamp { set; get; }

        public int no { set; get; }

        public int state { set; get; }

        public string createUserMail { set; get; }

        public DateTime createTime { set; get; }


        public long createTimestamp { set; get; }
    }
}
