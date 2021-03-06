﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private readonly IEventProcessor m_EventProcessor;

        public EventController(IEventProcessor eventProcessor)
        {
            m_EventProcessor = eventProcessor;
        }

        /// <summary>
        /// Process a user metrics event request
        /// </summary>
        [HttpPost, Route("track/v1/{organization}/{productId}/event")]
        public async Task<HttpResponseMessage> ReceiveEvent(string organization, string productId)
        {
            try
            {
                Trace.TraceInformation("Receiving event");

                // Sanity!
                if (m_EventProcessor == null)
                {
                    throw new HttpException((int) HttpStatusCode.InternalServerError,
                        "Event processor is not initialised");
                }

                // Pass on the message to the event processor
                string payload = await Request.Content.ReadAsStringAsync();

                bool eventWasProcessed = await m_EventProcessor.Process(organization, productId, payload);

                Trace.WriteLine(String.Format("Was event processed? [{0}", eventWasProcessed));

                return new HttpResponseMessage(eventWasProcessed ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                Trace.TraceError("Exception while processing event: {0}", e);
                throw;
            }
        }
    }
}