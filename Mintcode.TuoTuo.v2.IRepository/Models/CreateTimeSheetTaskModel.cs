using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository.Models
{
    public class CreateTimeSheetTaskModel
    {
        public string detail { set; get; }

        public int projectID { set; get; }

        public decimal time { set; get; }

    }
}
