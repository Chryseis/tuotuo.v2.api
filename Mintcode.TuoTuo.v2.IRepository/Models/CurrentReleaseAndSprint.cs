using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class CurrentReleaseAndSprint
    {
        public int releaseID { set; get; }

        public string releaseName { set; get; }

        public int sprintID { set; get; }

        public int sprintNo { set; get; }

        public long startTimestamp { set; get; }

        public long endTimestamp { set; get; }

    }
}
