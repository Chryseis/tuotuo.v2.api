using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class TaskRepoModel
    {
        public TaskInfoModel info { get; set; }

        public List<TaskLogModel> logs { get; set; }

    }
}
