using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class TimeSheetReportModel
    {
        public int teamMemberCount { set; get; }
    
        public int projectCount { set; get; }

        public decimal totalTime { set; get; }

        public List<TimeSheetReportUserModel> userInfos { set; get; }

    }
    public class TimeSheetReportUserModel
    {
        public int userID { set; get; }

        public string userMail { set; get; }

        public string userName { set; get; }

        public List<TimeSheetReportProjectModel> timeSheetTimeInfos { set; get; }

    }
    public class TimeSheetReportProjectModel
    {
        public int projectID { set; get; }
        
        public string projectName { set; get; }
        
        public decimal totalTime { set; get; }    
    }
}
