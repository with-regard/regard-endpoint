using System.Web.Http;
using Microsoft.Owin.Cors;
using Ninject;
using Owin;
using WebApiContrib.IoC.Ninject;

namespace Regard.Endpoint
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure for attribute routes
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.DependencyResolver = new NinjectResolver(new StandardKernel(new NinjectSettings
            {
                InjectNonPublic = true
            }, new EndpointModule()));
            httpConfiguration.MapHttpAttributeRoutes();

            app.UseCors(CorsOptions.AllowAll)
                .UseWebApi(httpConfiguration)
                .Run(async context =>
                           {
                               context.Response.ContentType = "text/plain";
                               await context.Response.WriteAsync("Hello, world. Again.");
                           });
        }
    }
}
