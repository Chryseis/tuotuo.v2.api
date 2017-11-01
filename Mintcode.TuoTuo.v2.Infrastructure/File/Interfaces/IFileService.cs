using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public interface IFileService
    {
        string UploadFile(string fileName,byte[]fileContent);

        void RemoveFile(string fileID);

        byte[] GetFile(string fileID);

        bool CheckFileExist(string fileID);
    }
}
