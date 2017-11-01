using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class ReleaseRepoModel
    {
        public ReleaseInfoModel releaseInfo { set; get; }

        public List<SprintInfoModel> sprintInfoList { set; get; }
    }
}
