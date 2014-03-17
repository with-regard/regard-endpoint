using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Regard.Endpoint
{
    /// <summary>
    /// 
    /// </summary>
    public class EventController : ApiController
    {
        /// <summary>
        /// Primary event processor
        /// </summary>
        private readonly IEventProcessor m_EventProcessor = new ServiceBusEventProcessor(new ServiceBusEventProcessorSettings());

        /// <summary>
        /// Process a user metrics event request
        /// </summary>
        [HttpPost, Route("track/v1/{organization}/{productId}/event")]
        public async Task<HttpResponseMessage> ReceiveEvent(string organization, string productId)
        {
            // Sanity!
            if (m_EventProcessor == null)
            {
                throw new HttpException((int) HttpStatusCode.InternalServerError, "Event processor is not initialised");
            }

            // Pass on the message to the event processor
            string payload = await Request.Content.ReadAsStringAsync();

            await m_EventProcessor.Process(organization, productId, payload);

            // Looks good
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}