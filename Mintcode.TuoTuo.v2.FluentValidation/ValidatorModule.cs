using Autofac;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.FluentValidation
{
    public class ValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {          
            AssemblyScanner.FindValidatorsInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
                .ForEach(x => builder.RegisterType(x.ValidatorType).As(x.InterfaceType).SingleInstance());
         
        }
    }
}
