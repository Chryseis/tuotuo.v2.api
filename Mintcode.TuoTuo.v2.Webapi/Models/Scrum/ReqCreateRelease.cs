using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models
{
    public class ReqCreateRelease : RequestBaseModel
    {
        public string releaseName { set; get; }

        public string releaseSummary { set; get; }
    }
}
