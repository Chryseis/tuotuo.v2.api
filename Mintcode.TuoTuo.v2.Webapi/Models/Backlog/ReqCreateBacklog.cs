using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models
{
    public class ReqCreateBacklog:RequestBaseModel
    {
       
        public string title { set; get; }

        public string content { set; get; }

        public string standard { set; get; }

        public string assignUserMail { set; get; }

        public int selectProjectID { set; get; }

        public int state { set; get; }

        public int? level { set; get; }

    }
}
