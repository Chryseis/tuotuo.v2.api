using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet
{
    public class ReqGetTimeSheetCheckList:RequestBaseModel
    {
        public long startTime { set; get; }

        public long endTime { set; get; }

        public List<int> selectTeamIDList { set; get; }

        public List<int> selectUserIDList { set; get; }

        public List<int> selectStatusList { set; get; }

        public int from { set; get; }

        public int to { set; get; }
    }
}
