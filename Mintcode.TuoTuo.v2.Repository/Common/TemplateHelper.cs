using Mintcode.TuoTuo.v2.Infrastructure;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Repository
{
    public class TemplateHelper
    {
        private IRazorEngineService _razorEngineService;

        public TemplateHelper(IRazorEngineService razorEngineService)
        {
            _razorEngineService = razorEngineService;
        }
        public string GenerateContent(string template,string name, object model)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                Enforce.Throw(new Exception("模板名称不能为空"));
            }
            if (_razorEngineService.IsTemplateCached(name, null))
            {
                result =_razorEngineService.Run(name, null, model);
            }
            else
            {
                if (string.IsNullOrEmpty(template))
                {
                    Enforce.Throw(new Exception("模板不能为空"));
                }
                result = _razorEngineService.RunCompile(template, name, null, model);
            }
            return result;

        }
    }
}
