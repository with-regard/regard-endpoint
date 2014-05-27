using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Regard.Endpoint
{
    internal class ServiceBusEventProcessor : IEventProcessor
    {
        private readonly IEventValidator m_EventValidator;
        private readonly TopicClient m_AnalyticsTopic;

        public ServiceBusEventProcessor(IServiceBusEventProcessorSettings settings, IEventValidator eventValidator)
        {
            m_EventValidator = eventValidator;
            var regardNamespace = NamespaceManager.CreateFromConnectionString(settings.ServiceBusConnectionString);

            if (!regardNamespace.TopicExists(settings.AnalyticsTopicName))
                regardNamespace.CreateTopic(settings.AnalyticsTopicName);

            TopicClient fromConnectionString = TopicClient.CreateFromConnectionString(settings.ServiceBusConnectionString, settings.AnalyticsTopicName);

            m_AnalyticsTopic = fromConnectionString;
        }

        public async Task<bool> Process(string organization, string product, string payload)
        {
            if (!m_EventValidator.IsValid(payload))
                return false;

            string serviceBusMessage = JsonConvert.SerializeObject(new
                                                                   {
                                                                       schema_version = 0x100,
                                                                       organization = organization,
                                                                       product = product,
                                                                       payload = payload
                                                                   });

            byte[] buffer = Encoding.UTF8.GetBytes(serviceBusMessage);

            using (var brokeredMessage = new BrokeredMessage(new MemoryStream(buffer, 0, buffer.Length, false, true), true))
            {
                await m_AnalyticsTopic.SendAsync(brokeredMessage);
            }

            return true;
        }
    }
}