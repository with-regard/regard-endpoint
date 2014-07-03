using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace Regard.Endpoint
{
    public class Deploymentcontroller : ApiController
    {
        [HttpGet, Route("deployment")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent(Assembly.GetExecutingAssembly().GetName().Version.ToString())
                   };
        }
    }
}