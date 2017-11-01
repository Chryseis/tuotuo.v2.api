using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Repository
{
    public class AttachmentHelper
    {
        public static string TransformExtensionToContentType(string extension)
        {
            string contentType = string.Empty;
            switch (extension)
            {
                case ".jpeg":
                case ".jpg":contentType = "image/jpeg"; break;      
                default:contentType = "application/octet-stream";break;
            }
            return contentType;
        }
    }
}
