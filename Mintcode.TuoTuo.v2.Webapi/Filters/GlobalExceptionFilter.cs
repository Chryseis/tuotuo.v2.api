using Autofac.Features.AttributeFilters;
using Autofac.Integration.WebApi;
using log4net;
using Mintcode.TuoTuo.v2.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class GlobalExceptionFilter : IAutofacExceptionFilter
    {
        private ILog _logger;

        public GlobalExceptionFilter([KeyFilter("ExceptionLog")] ILog logger)
        {
            this._logger = logger;
        }

        public Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            var responseContent = new ResponseBaseModel();

            if (actionExecutedContext.Exception is LogicErrorException)
            {
                responseContent.SetResponse(ResStatusCode.LogicError, actionExecutedContext.Exception.Message);
            }
            else if (actionExecutedContext.Exception is UnAuthorizeException)
            {
                responseContent.SetResponse(ResStatusCode.UnAuthorize, actionExecutedContext.Exception.Message);
            }
            else if (actionExecutedContext.Exception is UnAuthenticateException)
            {
                responseContent.SetResponse(ResStatusCode.UnAuthenticate, actionExecutedContext.Exception.Message);
            }
            else if(actionExecutedContext.Exception is FrontInputValidateErrorException)
            {
                responseContent.SetResponse(ResStatusCode.FrontInputValidateError, actionExecutedContext.Exception.Message);
            }
            else if (actionExecutedContext.Exception is UserInputValidateErrorException)
            {
                responseContent.SetResponse(ResStatusCode.UserInputValidateError, actionExecutedContext.Exception.Message);
            }
            else
            {
                responseContent.SetResponse(ResStatusCode.InternalServerError, "服务器内部错误");

                _logger.Error(JsonConvert.SerializeObject(new { uri = actionExecutedContext.Request.RequestUri, head = actionExecutedContext.Request.Headers, content = actionExecutedContext.Request.Content }), actionExecutedContext.Exception);
            }
            response.Content = new StringContent(JsonConvert.SerializeObject(responseContent));
            actionExecutedContext.Response = response;
            actionExecutedContext.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
            return Task.FromResult(0);
        }
    }
}
