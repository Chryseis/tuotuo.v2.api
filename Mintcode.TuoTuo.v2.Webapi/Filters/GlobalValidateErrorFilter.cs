using Autofac.Integration.WebApi;
using Mintcode.TuoTuo.v2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class GlobalValidateErrorFilter : IAutofacActionFilter
    {
        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!actionContext.ModelState.IsValid)
            {
                IDictionary<string, IEnumerable<string>> errors = new Dictionary<string, IEnumerable<string>>();
                ResponseBaseModel responseContent = new ResponseBaseModel();
                responseContent.code = ResStatusCode.UserInputValidateError;
                foreach (var keyModelStatePair in actionContext.ModelState)
                {
                    if (keyModelStatePair.Value.Errors != null && keyModelStatePair.Value.Errors.Count > 0)
                    {
                        IEnumerable<string> errorMessages = keyModelStatePair.Value.Errors
                            .Where(s => !string.IsNullOrEmpty(s.ErrorMessage))
                            .Select(s => s.ErrorMessage).ToList();
                        string key = keyModelStatePair.Key;
                        if (keyModelStatePair.Key.Split('.').Count()>0)
                        {
                            key = string.Join(".", keyModelStatePair.Key.Split('.').Skip(1));
                        }
                        errors.Add(key, errorMessages);
                    }
                }

                //暂时性先显示第一个错误
                //if (errors.Count > 0)
                //{
                //    responseContent.message = errors.First().Value.First();
                //}
                responseContent.message = Newtonsoft.Json.JsonConvert.SerializeObject(errors);

                actionContext.Response = new System.Net.Http.HttpResponseMessage();
                actionContext.Response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(responseContent));
                actionContext.Response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                actionContext.Response.Content.Headers.ContentEncoding.Add("utf-8");
            }
            return Task.FromResult(0);
        }
    }
}
