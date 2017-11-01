using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class TimeSheetQueryModel
    {
       public int teamID { set; get; }

       public string teamName { set; get; }

       public List<TimeSheetQueryUserModel> teamMembers { set; get; }
       
    }

    public class TimeSheetQueryUserModel
    {
        public int userID { set; get; }

        public string userMail { set; get; }

        public string userName { set; get; }

        public List<TimeSheetQueryProjectModel> userProjects { set; get; }

    }

    public class TimeSheetQueryProjectModel
    {
         public int projectID { set; get; }

        public string projectName { set; get; }
    }
}
