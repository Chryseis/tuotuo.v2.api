using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Repository
{
    public class AttachmentUploadRepository: IAttachmentUploadRepository
    {
        private string prefix = "Attachment:Upload:";

        private IDatabase database;
        public AttachmentUploadRepository(IDatabase _database)
        {
            database = _database;
        }

        public async Task<bool> InsertAttachmentUploadToken(string token, AttachmentUploadModel attachmentUploadModel, TimeSpan? expiry)
        {
            Enforce.ArgumentNotNull<string>(token, "Upload Token 不能为null");
            Enforce.ArgumentNotNull<AttachmentUploadModel>(attachmentUploadModel, "AttachmentUploadModel 不能为null");

            string key = string.Concat(prefix, token);
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(attachmentUploadModel);
            bool result = await database.StringSetAsync(key, value, expiry);
            return result;

        }

        public async Task<string> GetAttachmentFileID(string token,string userMail)
        {
            string fileID = string.Empty;
            Enforce.ArgumentNotNull<string>(token, "Token 不能为null");
        
            string key = string.Concat(prefix, token);

            string model = await this.database.StringGetAsync(key);

            //await this.database.KeyDeleteAsync(key);

            if (!string.IsNullOrEmpty(model))
            {
                AttachmentUploadModel attachmentUploadModel = Newtonsoft.Json.JsonConvert.DeserializeObject<AttachmentUploadModel>(model);

                if (attachmentUploadModel != null && attachmentUploadModel.userMail.Equals(userMail))
                {
                    fileID = attachmentUploadModel.fileID;
                }

            }
           
            return fileID;
        }
    }
}
