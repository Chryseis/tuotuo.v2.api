using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public class LocalFileService : IFileService
    {
        private string filePath;

        public LocalFileService()
        {
            filePath = ConfigurationManager.AppSettings["attachment"];
        }
        public byte[] GetFile(string fileID)
        {  
            string fullFilePathName = GetFullFilePathName(filePath, fileID);

            Enforce.That(File.Exists(fullFilePathName), "文件不存在或已被删除");

            var bytes = File.ReadAllBytes(fullFilePathName);

            return bytes;
        }

        public void RemoveFile(string fileID)
        {
            string fullFilePathName = GetFullFilePathName(filePath, fileID);
            if (File.Exists(fullFilePathName))
            {
                File.Delete(fullFilePathName);
            }        
        }

        public string UploadFile(string fileName, byte[] fileContent)
        {
            Enforce.That(!(null == fileContent || fileContent.Count() <= 0), "文件内容不能为空");

            string fullFilePathName = GetFullFilePathName(filePath, fileName);

            Enforce.That(!File.Exists(fullFilePathName),new Exception("文件已经存在"));

            System.IO.FileInfo file = new System.IO.FileInfo(fullFilePathName);
            file.Directory.Create();
            File.WriteAllBytes(fullFilePathName,fileContent);
            
            return fileName;
        }

        public bool CheckFileExist(string fileID)
        {
            string fullFilePathName = GetFullFilePathName(filePath, fileID);
            return File.Exists(fullFilePathName);

        }

        /// <summary>
        /// 根据路径和文件名创建完整的文件名称
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFullFilePathName(string filePath,string fileName)
        {
            Enforce.That(!string.IsNullOrEmpty(filePath), "附件路径不能为空");

            Enforce.That(!string.IsNullOrEmpty(fileName), "文件名称不能为空");

            string fullFilePathName = string.Concat(filePath.TrimEnd('/'),
            fileName.StartsWith("/") ? fileName : string.Concat("/", fileName));
            return fullFilePathName;
        }
    }
}
