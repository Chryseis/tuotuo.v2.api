using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class TimeSheetTaskModel
    {
        public int ID { set; get; }

        public int sheetID { set; get; }

        public int projectID { set; get; }

        public string projectName { set; get; }

        public string detail { set; get; }

        public decimal time { set; get; }

        public int userID { set; get; }

        public string userMail { set; get; }

        public string userName { set; get; }

        public string createUserMail { set; get; }

        public DateTime createTime { set; get; }

        public long createTimestamp { set; get; }
    }
}
