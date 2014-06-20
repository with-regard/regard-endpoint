using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Regard.Endpoint
{
    internal class ServiceBusClient : IServiceBusClient
    {
        private readonly TopicClient m_AnalyticsTopic;
        
        public ServiceBusClient(IServiceBusEventProcessorSettings settings)
        {
            var regardNamespace = NamespaceManager.CreateFromConnectionString(settings.ServiceBusConnectionString);

            if (!regardNamespace.TopicExists(settings.AnalyticsTopicName))
                regardNamespace.CreateTopic(settings.AnalyticsTopicName);

            m_AnalyticsTopic = TopicClient.CreateFromConnectionString(settings.ServiceBusConnectionString, settings.AnalyticsTopicName);
        }

        public async Task Post(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            using (var brokeredMessage = new BrokeredMessage(new MemoryStream(buffer, 0, buffer.Length, false, true), true))
            {
                await m_AnalyticsTopic.SendAsync(brokeredMessage);
            }
        }
    }
}