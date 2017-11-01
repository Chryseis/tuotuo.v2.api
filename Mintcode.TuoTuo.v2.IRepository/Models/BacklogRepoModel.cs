using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class BacklogRepoModel
    {
        public BacklogInfoModel info { get; set; }

        public List<TaskInfoModel> tasks { get; set; }
    }
}
