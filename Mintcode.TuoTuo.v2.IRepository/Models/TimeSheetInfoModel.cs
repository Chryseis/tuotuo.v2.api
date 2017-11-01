using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class TimeSheetInfoModel
    {
        public int ID { set; get; }

        public int userID { set; get; }

        public string userMail { set; get; }

        public string userName { set; get; }

        public DateTime timeSheetDate { set; get; }

        public long timeSheetTimeStamp { set; get; }

        public int status { set; get; }

        public DateTime? submitTime { set; get; }

        public int? approvalUserID { set; get; }

        public string approvalUserMail { set; get; }

        public string approvalUserName { set; get; }

        public DateTime? approvalTime { set; get; }

        public int? approvalResult { set; get; }

        public string approvalComment { set; get; }

        public string createUserMail { set; get; }

        public DateTime createTime { set; get; }

        public long createTimestamp { set; get; }
    }
}
