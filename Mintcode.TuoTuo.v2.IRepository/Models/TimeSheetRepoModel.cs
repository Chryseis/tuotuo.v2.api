using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class TimeSheetRepoModel
    {
        public TimeSheetInfoModel info { set; get; }

        public List<TimeSheetTaskModel> tasks { set; get; }
    }
}
