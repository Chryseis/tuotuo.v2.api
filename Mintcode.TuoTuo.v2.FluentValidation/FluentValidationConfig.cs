using FluentValidation;
using FluentValidation.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;

namespace Mintcode.TuoTuo.v2.FluentValidation
{
    public class FluentValidationConfig
    {
        public static void ConfigureContainer(HttpConfiguration config)
        {
            FluentValidationModelValidatorProvider.Configure(config,provider=> {
                provider.ValidatorFactory = new DependencyResolverValidatorFactory(config); 
                ValidatorOptions.CascadeMode = CascadeMode.Continue;
            });
        }
    }
}
