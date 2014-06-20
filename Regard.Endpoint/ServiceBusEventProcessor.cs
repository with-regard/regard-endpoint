using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Regard.Endpoint
{
    internal class ServiceBusEventProcessor : IEventProcessor
    {
        private readonly IServiceBusClient m_Client;
        private readonly IPayloadValidator m_EventValidator;
        
        public ServiceBusEventProcessor(IServiceBusClient client, IPayloadValidator payloadValidator)
        {
            m_Client = client;
            m_EventValidator = payloadValidator;
        }

        public async Task<bool> Process(string organization, string product, string payload)
        {
            IEnumerable<string> events;
            if (!m_EventValidator.TryGetEvents(payload, out events))
                return false;

            await Task.WhenAll(events.Select(async x => await Send(organization, product, x)));

            return true;
        }

        public async Task Send(string organization, string product, string singleEventPayload)
        {
            string serviceBusMessage = JsonConvert.SerializeObject(new
                                                                   {
                                                                       schema_version = 0x100,
                                                                       organization = organization,
                                                                       product = product,
                                                                       payload = singleEventPayload
                                                                   });

            await m_Client.Post(serviceBusMessage);
        }
    }
}