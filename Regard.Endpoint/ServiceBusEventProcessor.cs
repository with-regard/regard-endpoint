using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Regard.Endpoint
{
    internal class ServiceBusEventProcessor : IEventProcessor
    {
        private readonly TopicClient m_AnalyticsTopic;

        public ServiceBusEventProcessor(IServiceBusEventProcessorSettings settings)
        {
            var regardNamespace = NamespaceManager.CreateFromConnectionString(settings.ServiceBusConnectionString);

            if (!regardNamespace.TopicExists(settings.AnalyticsTopicName))
                regardNamespace.CreateTopic(settings.AnalyticsTopicName);

            TopicClient fromConnectionString = TopicClient.CreateFromConnectionString(settings.ServiceBusConnectionString, settings.AnalyticsTopicName);

            m_AnalyticsTopic = fromConnectionString;
        }

        public async Task Process(string organization, string product, string payload)
        {
            string serviceBusMessage = JsonConvert.SerializeObject(new
                                                                   {
                                                                       organization = organization,
                                                                       product = product,
                                                                       payload = payload
                                                                   });

            var brokeredMessage = new BrokeredMessage(serviceBusMessage);
                
            await m_AnalyticsTopic.SendAsync(brokeredMessage);
        }
    }
}