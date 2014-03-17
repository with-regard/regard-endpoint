using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Regard.Endpoint
{
    internal class ServiceBusEventProcessor : IEventProcessor
    {
        private readonly TopicClient m_AnalyticsTopic;

        public ServiceBusEventProcessor()
        {
            var regardNamespace = NamespaceManager.CreateFromConnectionString(c_ConnectionString);

            if (!regardNamespace.TopicExists(c_AnalyticsTopic))
                regardNamespace.CreateTopic(c_AnalyticsTopic);

            TopicClient fromConnectionString = TopicClient.CreateFromConnectionString(c_ConnectionString, c_AnalyticsTopic);

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