using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.Infrastructure.Util;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mintcode.TuoTuo.v2.Webapi.Controllers
{
    [RoutePrefix("attachment")]
    public class AttachmentController : BaseController
    {
        private IFileService _fileService;
        private IAttachmentUploadRepository _attachmentUploadRepository;
        public AttachmentController(IFileService fileService, IAttachmentUploadRepository attachmentUploadRepository, IdentityService identityService) 
            : base(identityService)
        {
            this._fileService = fileService;
            this._attachmentUploadRepository = attachmentUploadRepository;
        }

        [Route("UploadImage")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<IHttpActionResult> UploadImage()
        {          
            var response = new ResUpload();
            var content =await this.ExtractContentFromMultipartContent();
            List<UploadResult> result = new List<UploadResult>();
            foreach (var fileContent in content.fileContents)
            {
                var uploadResult= await this.Upload(fileContent);
                if (uploadResult!=null)
                {
                    result.Add(uploadResult);
                }
            }
            response.setResponse(ResStatusCode.OK, result, result.Count);
            return Ok(response);
           

        }

        [Route("UploadFile")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<IHttpActionResult> UploadFile()
        {
            var response = new ResUpload();
            var content = await this.ExtractContentFromMultipartContent();
            List<UploadResult> result = new List<UploadResult>();
            foreach (var fileContent in content.fileContents)
            {
                var uploadResult = await this.Upload(fileContent);
                if (uploadResult != null)
                {
                    result.Add(uploadResult);
                }
            }
            response.setResponse(ResStatusCode.OK, result, result.Count);
            return Ok(response);


        }

        #region 私有方法

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="isCompressImage">是否需要压缩图片</param>
        /// <returns></returns>
        private async Task<UploadResult> Upload(HttpContent fileContent)
        {
            UploadResult uploadResult = null;
            var userClaimsInfoModel = this.GetUserModelFromCurrentClaimsIdentity();

            string fileName = fileContent.Headers.ContentDisposition.FileName.Trim('"');
            string newFileName = string.Concat(
                DateTime.Now.ToString("yyyyMMdd"), "_", Guid.NewGuid().ToString(), "_", fileName);
            var saveBytes = await fileContent.ReadAsByteArrayAsync();   
            //暂时性先放到一个文件夹下面去
            string fileID = _fileService.UploadFile(newFileName, saveBytes);



            string token = Guid.NewGuid().ToString();
            AttachmentUploadModel attachmentUploadModel = new AttachmentUploadModel();
            attachmentUploadModel.fileID = fileID;
            attachmentUploadModel.userMail = userClaimsInfoModel.mail;
            if (await _attachmentUploadRepository.InsertAttachmentUploadToken(token, attachmentUploadModel, new TimeSpan(1, 0, 0)))
            {
                uploadResult = new UploadResult();
                uploadResult.uploadToken = token;
            }

            return uploadResult;
        }

        /// <summary>
        /// 从MultipartContent中提取上传文件内容和参数
        /// </summary>
        /// <returns></returns>
        private async Task<MultipartContentModel> ExtractContentFromMultipartContent()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            NameValueCollection formData = new NameValueCollection();
            List<HttpContent> fileContents = new List<HttpContent>();
            foreach (var content in provider.Contents)
            {

                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    fileContents.Add(content);
                }
                else
                {
                    string formFieldName = content.Headers.ContentDisposition.Name;
                    formFieldName = string.IsNullOrEmpty(formFieldName) ? formFieldName : formFieldName.Trim('"');
                    string formFieldValue = await content.ReadAsStringAsync();
                    formData.Add(formFieldName, formFieldValue);
                }
            }
            MultipartContentModel model = new MultipartContentModel();
            model.formData = formData;
            model.fileContents = fileContents;
            return model;

        }



        private class MultipartContentModel
        {
            public NameValueCollection formData { set; get; }

            public List<HttpContent> fileContents { set; get; }
        }

        #endregion




    }
}
