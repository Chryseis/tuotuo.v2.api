using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
   public  class SearchTeamAndTeamMemberModel
    {
       public IList<TeamRepoModel> teamInfos { get; set; }

       public IList<TeamMemberModel> teamMembers { get; set; }

    }
}
