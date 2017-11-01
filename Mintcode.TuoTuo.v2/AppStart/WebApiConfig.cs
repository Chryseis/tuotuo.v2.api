using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Features.AttributeFilters;
using log4net;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Repository;
using Mintcode.TuoTuo.v2.AutoMapper;
using Mintcode.TuoTuo.v2.Webapi;
using Mintcode.TuoTuo.v2.Webapi.Controllers;

namespace Mintcode.TuoTuo.v2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //注册对象映射
            Configuration.Configure();

            //跨域开启
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //属性路由
            config.MapHttpAttributeRoutes();

            //默认路由
            config.Routes.MapHttpRoute(name: "default", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });

           
            //autofac
            var builder = new ContainerBuilder();
            //日志
            builder.Register(c => LogManager.GetLogger("GlobalLog")).Keyed("GlobalLog", typeof(ILog));
            builder.Register(c => LogManager.GetLogger("ExceptionLog")).Keyed("ExceptionLog", typeof(ILog));
            //filter
            //builder.RegisterType<GlobalAuthenticationFilter>().AsWebApiAuthenticationFilterFor<BaseController>().InstancePerRequest();
            builder.RegisterType<GlobalActionFilter>().AsWebApiActionFilterFor<BaseController>().InstancePerRequest();
            builder.RegisterType<GlobalExceptionFilter>().WithAttributeFiltering().AsWebApiExceptionFilterFor<BaseController>().InstancePerRequest();
            //logic
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            //api
            builder.RegisterApiControllers(Assembly.Load("Mintcode.TuoTuo.v2.Webapi")).WithAttributeFiltering();
            builder.RegisterWebApiFilterProvider(config);
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}