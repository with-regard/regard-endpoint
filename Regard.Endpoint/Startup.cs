using System.Web.Http;
using Owin;

namespace Regard.Endpoint
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure for attribute routes
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            app.UseWebApi(httpConfiguration);

            // New code:
            app.Run(async context =>
                          {
                              context.Response.ContentType = "text/plain";
                              await context.Response.WriteAsync("Hello, world.");
                          });
        }
    }
}
