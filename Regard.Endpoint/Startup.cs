using System.Net;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Ninject;
using Owin;

namespace Regard.Endpoint
{
    public class Startup
    {
        private readonly EndpointModule m_Module;

        public Startup()
            : this(new EndpointModule())
        {
        }

        public Startup(EndpointModule module)
        {
            m_Module = module;
        }

        public void Configuration(IAppBuilder app)
        {
            // Configure for attribute routes
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.DependencyResolver = new NinjectResolver(new StandardKernel(new NinjectSettings
            {
                InjectNonPublic = true
            }, m_Module));
            httpConfiguration.MapHttpAttributeRoutes();

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(httpConfiguration);
            app.Run(async x =>
                          {
                              switch (x.Request.Method)
                              {
                                  case "GET":
                                      x.Response.Redirect("https://withregard.io");
                                      break;
                                  default:
                                      x.Response.StatusCode = (int) HttpStatusCode.NotFound;
                                      break;
                              }
                          });

        }
    }
}
