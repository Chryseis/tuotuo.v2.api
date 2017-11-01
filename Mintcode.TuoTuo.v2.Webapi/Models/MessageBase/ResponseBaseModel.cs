using Mintcode.TuoTuo.v2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ResponseBaseModel
    {
        public ResStatusCode code { get; set; }

        public string message { get; set; }

        public void SetResponse(ResStatusCode code, string message = "")
        {
            this.code = code;
            this.message = message;
        }
    }

    public class ResponseBaseModel<T> : ResponseBaseModel where T : class,new()
    {
        public T data { get; set; }

        public long totalCount { get; set; }

        public void setResponse(ResStatusCode code, T data, long totalCount = 0, string message = "")
        {
            this.code = code;
            this.data = data;
            this.totalCount = totalCount;
            this.message = message;
        }
    }
}
