using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Models
{
    public class ResUpload : ResponseBaseModel<List<UploadResult>>
    {

    }

    public class UploadResult
    {
        public string uploadToken { set; get; }
    }
}
