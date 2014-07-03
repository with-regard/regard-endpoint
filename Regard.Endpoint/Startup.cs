using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Exceptionless;
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

        //Tests can get to this, but helios loader can't. Helios will try to do DI on public constructors. Weird.
        internal Startup(EndpointModule module)
        {
            m_Module = module;
        }

        public void Configuration(IAppBuilder app)
        {
            Trace.TraceInformation("Regard Endpoint is starting");

            // Configure for attribute routes
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.DependencyResolver = new NinjectResolver(new StandardKernel(new NinjectSettings
            {
                InjectNonPublic = true
            }, m_Module));
            httpConfiguration.MapHttpAttributeRoutes();

            bool exceptionlessEnabled;
            if (Boolean.TryParse(ConfigurationManager.AppSettings.Get("regard-exceptionless-enabled"), out exceptionlessEnabled) && exceptionlessEnabled)
            {
                ExceptionlessClient.Current.RegisterWebApi(httpConfiguration);
            }

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(httpConfiguration);
            app.Run(async x =>
                    {
                        switch (x.Request.Method)
                        {
                            case "GET":
                                Trace.TraceInformation("Redirect to withregard.io from {0}", x.Request.Uri);
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
