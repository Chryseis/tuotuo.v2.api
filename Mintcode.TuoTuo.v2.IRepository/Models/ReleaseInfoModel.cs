using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class ReleaseInfoModel
    {

        public int ID { set; get; }

        public int teamID { set; get; }

        public string releaseName { set; get; }

        public string releaseSummary { set; get; }

       
        public string createUserMail { set; get; }

        public DateTime createTime { set; get; }

        public long createTimestamp { set; get; }

    }
}
