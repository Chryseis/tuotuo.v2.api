using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Web.Http.SelfHost;
using System.Web.Http;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Features.AttributeFilters;
using log4net;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.Webapi;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Repository;
using Topshelf;
using Mintcode.TuoTuo.v2.Webapi.Controllers;

namespace Mintcode.TuoTuo.v2.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8000/");
            //开启跨域
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //属性路由
            config.MapHttpAttributeRoutes();

            //路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.EnsureInitialized();

            //autofac
            var builder = new ContainerBuilder();
            //日志
            builder.Register(c => LogManager.GetLogger("GlobalLog")).Keyed("GlobalLog", typeof(ILog));
            builder.Register(c => LogManager.GetLogger("ExceptionLog")).Keyed("ExceptionLog", typeof(ILog));
            //身份认证filter
            //builder.RegisterType<GlobalAuthenticationFilter>().AsWebApiAuthenticationFilterFor<BaseController>().InstancePerRequest();
            //actionfilter
            builder.RegisterType<GlobalActionFilter>().AsWebApiActionFilterFor<BaseController>().InstancePerRequest();
            //异常filter
            builder.RegisterType<GlobalExceptionFilter>().AsWebApiExceptionFilterFor<BaseController>().InstancePerRequest();
            //logic
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            //api
            builder.RegisterApiControllers(Assembly.Load("Mintcode.TuoTuo.v2.Webapi")).WithAttributeFiltering();
            builder.RegisterWebApiFilterProvider(config);
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync();
                Console.WriteLine("listen:");
                Console.WriteLine("http://localhost:8000/");
                Console.ReadLine();
            }
        }
    }
}
