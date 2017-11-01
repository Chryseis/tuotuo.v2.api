using Mintcode.TuoTuo.v2.IRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models.TimeSheet
{
    public class ReqCreateTimeSheetTasks : RequestBaseModel
    {
        public int sheetID { set; get; }

        public List<CreateTimeSheetTaskModel> tasks { set; get; }
    }

   
}
