using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public class AuthCodeModel
    {
        public Stream Img { get; set; }

        public string Code { get; set; }
    }
}
