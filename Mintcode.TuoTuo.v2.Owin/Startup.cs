using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Cors;
using Autofac;
using log4net;
using Mintcode.TuoTuo.v2.Infrastructure;
using System.Reflection;
using Autofac.Integration.WebApi;
using Autofac.Features.AttributeFilters;
using Mintcode.TuoTuo.v2.Repository;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.BLL;
using Microsoft.Owin.Cors;
using System.Configuration;
using StackExchange.Redis;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Mintcode.TuoTuo.v2.FluentValidation;
using MailKit.Net.Smtp;
using System.Web.Http.Hosting;
using Mintcode.TuoTuo.v2.Webapi;
using Mintcode.TuoTuo.v2.Webapi.Controllers;
using Mintcode.TuoTuo.v2.Webapi.Identity;
using Mintcode.TuoTuo.v2.Webapi.Identity.CAS;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using log4net.Config;

[assembly: OwinStartup(typeof(Mintcode.TuoTuo.v2.Owin.Startup))]

namespace Mintcode.TuoTuo.v2.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888

            //log4net 加载配置
            XmlConfigurator.Configure();

            var config = new HttpConfiguration();

            //注册对象映射
            Mintcode.TuoTuo.v2.AutoMapper.Configuration.Configure();

            //属性路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            FluentValidationConfig.ConfigureContainer(config);

            //autofac
            var builder = new ContainerBuilder();
            //日志
            builder.Register(c => LogManager.GetLogger("GlobalLog")).Keyed("GlobalLog", typeof(ILog));
            builder.Register(c => LogManager.GetLogger("ExceptionLog")).Keyed("ExceptionLog", typeof(ILog));
            //filter
            //builder.RegisterType<GlobalAuthenticationFilter>().AsWebApiAuthenticationFilterFor<BaseController>().InstancePerRequest();
            builder.RegisterType<GlobalActionFilter>().AsWebApiActionFilterFor<BaseController>().InstancePerRequest();
            builder.RegisterType<GlobalValidateErrorFilter>().AsWebApiActionFilterFor<BaseController>().InstancePerRequest();
            builder.RegisterType<GlobalExceptionFilter>().WithAttributeFiltering().AsWebApiExceptionFilterFor<BaseController>().InstancePerRequest();
            //logic
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeamRepository>().As<ITeamRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TimeSheetRepository>().As<ITimeSheetRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TaskRepository>().As<ITaskRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RefreshTokenRepository>().As<IRefreshTokenRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RelationAccountRepository>().As<IRelationAccountRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AttachmentUploadRepository>().As<IAttachmentUploadRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BacklogRepository>().As<IBacklogRepository>()
                .OnActivated(s=>s.Instance.SetTaskRepository(s.Context.Resolve<ITaskRepository>())).InstancePerLifetimeScope();
            builder.RegisterType<ScrumRepository>().As<IScrumRepository>()
               .OnActivated(s => s.Instance.SetBacklogRepository(s.Context.Resolve<IBacklogRepository>())).InstancePerLifetimeScope();
            //BLL
            var assembly = Assembly.Load("Mintcode.TuoTuo.v2.BLL");
             builder.RegisterAssemblyTypes(assembly).Where(t => 
             t.BaseType.GetGenericTypeDefinition().Equals(typeof(BLLBase<>)));
            //api
            builder.RegisterApiControllers(Assembly.Load("Mintcode.TuoTuo.v2.Webapi")).WithAttributeFiltering();
            builder.RegisterWebApiFilterProvider(config);

            //redis
            builder.Register(c =>
            {
                var redisConfig = ConfigurationManager.ConnectionStrings["redis"].ConnectionString;
                var options = ConfigurationOptions.Parse(redisConfig);
                options.AbortOnConnectFail = false;
                var connection = ConnectionMultiplexer.Connect(options);
                connection.PreserveAsyncOrder = false;
                return connection;
            }).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<ConnectionMultiplexer>().GetDatabase()).As<IDatabase>();

            //OAuth 逻辑
            builder.RegisterType<OpenRefreshTokenProvider>();
            builder.RegisterType<SimpleAuthorizationServerProvider>();
            builder.Register((c, p) => new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(int.Parse(ConfigurationManager.AppSettings["AccessTokenExpireDay"])),

                AccessTokenFormat = new TicketDataFormat(app.CreateDataProtector(
                                                          typeof(OAuthAuthorizationServerMiddleware).Namespace,
                                                          "Access_Token", "v1")),
                Provider = c.Resolve<SimpleAuthorizationServerProvider>(),

                RefreshTokenFormat = new TicketDataFormat(app.CreateDataProtector(
                                                          typeof(OAuthAuthorizationServerMiddleware).Namespace,
                                                          "Refresh_Token", "v1")),
                RefreshTokenProvider = c.Resolve<OpenRefreshTokenProvider>()
            }).SingleInstance();
            builder.RegisterType<IdentityService>()
                .OnActivated(s => s.Instance.options = s.Context.Resolve<OAuthAuthorizationServerOptions>());

            //Api Handlers   
            builder.RegisterType<CASOAuthHandler>().OnActivating(e =>
            {
                e.Instance.SetIdentityService(e.Context.Resolve<IdentityService>());
                e.Instance.SetUserRepository(e.Context.Resolve<IUserRepository>());
                e.Instance.SetRelationAccountRepository(e.Context.Resolve<IRelationAccountRepository>());
            });

            //Email
            #region 废弃代码
            //builder.Register(c => {
            //    var mailConfig = ConfigurationManager.GetSection("mailConfig") as MailConfigurationSection;
            //    return mailConfig;
            //}).AsSelf().SingleInstance();
            //builder.Register(async (c, p) =>
            //{
            //    try
            //    {
            //        var mailConfig = c.Resolve<MailConfigurationSection>();
            //        var smtpClient = new MailKit.Net.Smtp.SmtpClient();

            //        smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");

            //        int port = int.Parse(mailConfig.Port.Text);
            //        await smtpClient.ConnectAsync(mailConfig.Server.Text, port, mailConfig.EnableSsl.Text.Equals("true"));
            //        await smtpClient.AuthenticateAsync(mailConfig.UserName.Text, mailConfig.Password.Text);

            //        return smtpClient;
            //    }
            //    catch (Exception ex)
            //    {
            //        return null;
            //    }

            //}).AsSelf().SingleInstance();
            //builder.Register((c, p) =>
            //{
            //    var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            //    smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //    return smtpClient;
            //}).AsSelf().SingleInstance();
            #endregion 
            builder.Register(c =>
            {
                var mailConfig = ConfigurationManager.GetSection("mailConfig") as TuoTuoMailConfigurationSection;
                return mailConfig;
            }).AsSelf().SingleInstance();
            builder.RegisterType<EmailService>().As<IEmailService>();

            //加密
            builder.RegisterType<JWTTokenSecurity>().As<ITokenSecurity>();


            //Validator
            builder.RegisterModule<ValidatorModule>();

            //文件
            builder.RegisterType<LocalFileService>().As<IFileService>();

            //模板
            builder.Register(c=> 
            {
                var templateConfig = new TemplateServiceConfiguration();
                templateConfig.DisableTempFileLocking = true;
                templateConfig.CachingProvider = new DefaultCachingProvider(t => { });
                return  RazorEngineService.Create(templateConfig);
            }).AsSelf().SingleInstance();
            builder.RegisterType<TemplateHelper>().SingleInstance();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseCors(CorsOptions.AllowAll);

            app.UseAutofacMiddleware(container);

            app.UseAutofacWebApi(config);

            //OAuth 配置
            app.UseOAuthAuthorizationServer(container.Resolve<OAuthAuthorizationServerOptions>());
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            app.UseWebApi(config);

            


            //第三方认证路由
            config.Routes.MapHttpRoute(
                    name: "CAS OAuth",
                    routeTemplate:"cas",
                    defaults:null,
                    constraints:null,
                    handler:container.Resolve<CASOAuthHandler>()
                );




        }


    }
}
