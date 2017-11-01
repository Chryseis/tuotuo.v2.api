using Mintcode.TuoTuo.v2.DFS;
using Mintcode.TuoTuo.v2.DFS.Common;
using Mintcode.TuoTuo.v2.DFS.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public class FastDFSService : IFileService
    {
        //C#支持的类库需要重新开发或者查找其他包替换(可以参考Java版本的包)
        private static FastDfsConfig config;
        private StorageNode storageNode;

        static FastDFSService()
        {
            config = FastDfsManager.GetConfigSection();
            ConnectionManager.InitializeForConfigSection(config);
        }

        public FastDFSService()
        {
            storageNode= FastDFSClient.GetStorageNode(config.GroupName);
        }

       
        public byte[] GetFile(string fileID)
        {
            return FastDFSClient.DownloadFile(storageNode,fileID);
        }

        public void RemoveFile(string fileID)
        {
            FastDFSClient.RemoveFile(config.GroupName, fileID);
        }

        public string UploadFile(string fileName, byte[] fileContent)
        {
            if (null == fileContent || fileContent.Count() <= 0)
            {
                Enforce.Throw(new Exception("文件内容不能为空"));
            }
            string fileExtension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(fileExtension))
            {
                Enforce.ArgumentNotNull<string>(fileExtension,"文件后缀名不能为空");
            }
            fileExtension = fileExtension.TrimStart('.');

            string fileID=FastDFSClient.UploadFile(storageNode, fileContent, fileExtension);

            return fileID;
        }

        public bool CheckFileExist(string fileID)
        {
            throw new NotImplementedException();
        }


    }
}
