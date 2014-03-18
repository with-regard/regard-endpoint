using System.Configuration;

namespace Regard.Endpoint
{
    internal class ConfigurationManagerBasedSettings : IServiceBusEventProcessorSettings
    {
        public string ServiceBusConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("regard-service-bus-conenction-string");
            }
        }

        public string AnalyticsTopicName
        {
            get { return ConfigurationManager.AppSettings.Get("regard-service-bus-topic-name"); }
        }
    }
}