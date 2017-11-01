using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public class ProjectRepoModel
    {
       public ProjectInfoModel info { set; get; }

        public List<ProjectMemberModel> members { set; get; }
    }
}
