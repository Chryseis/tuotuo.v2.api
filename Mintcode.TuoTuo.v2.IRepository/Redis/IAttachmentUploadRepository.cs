using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface IAttachmentUploadRepository
    {
        Task<bool> InsertAttachmentUploadToken(string token, AttachmentUploadModel attachmentUploadModel, TimeSpan? expiry);

        Task<string> GetAttachmentFileID(string token, string userMail);

    }
}
