using System.Configuration;

namespace Regard.Endpoint
{
    internal class ConfigurationManagerBasedSettings : IServiceBusEventProcessorSettings
    {
        private readonly string m_ServiceBusConnectionString;
        private readonly string m_AnalyticsTopicName;

        public ConfigurationManagerBasedSettings()
        {
            m_ServiceBusConnectionString = ConfigurationManager.AppSettings.Get("regard-service-bus-conenction-string");
            m_AnalyticsTopicName = ConfigurationManager.AppSettings.Get("regard-service-bus-topic-name");
        }

        public string ServiceBusConnectionString
        {
            get { return m_ServiceBusConnectionString; }
        }

        public string AnalyticsTopicName
        {
            get { return m_AnalyticsTopicName; }
        }
    }
}