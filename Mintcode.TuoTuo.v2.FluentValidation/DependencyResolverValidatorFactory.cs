using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mintcode.TuoTuo.v2.FluentValidation
{
    public class DependencyResolverValidatorFactory: ValidatorFactoryBase
    {
        private HttpConfiguration config;
        public DependencyResolverValidatorFactory(HttpConfiguration _config)
        {
            config = _config;
        }
        public override IValidator CreateInstance(Type validatorType)
        {
            return config.DependencyResolver.GetService(validatorType) as IValidator;
        }
    }
}
