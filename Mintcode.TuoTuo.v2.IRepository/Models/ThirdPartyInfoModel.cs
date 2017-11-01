using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class ThirdPartyInfoModel
    {
        public int ID { set; get; }

        public int userID { set; get; }

        public string thirdPartyID { set; get; }

        public string from { set; get; }

        public DateTime createTime { set; get; }

        public long createTimestamp { set; get; }
    }
}
