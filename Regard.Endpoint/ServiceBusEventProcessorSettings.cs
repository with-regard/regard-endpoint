using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Regard.Endpoint
{
    internal class ServiceBusEventProcessorSettings : IServiceBusEventProcessorSettings
    {
        public string ServiceBusConnectionString
        {
            get { return (string) GetAzureSettingsObject()["ServiceBusConnectionString"]; }
        }

        public string AnalyticsTopicName
        {
            get { return (string) GetAzureSettingsObject()["ServiceBusTopicName"]; }
        }

        private JObject GetAzureSettingsObject()
        {
            return JObject.Parse(GetEmbeddedAzureSettings());
        }

        private string GetEmbeddedAzureSettings()
        {
            Stream manifestResourceStream = Assembly.GetCallingAssembly().GetManifestResourceStream("Regard.Endpoint.AzureSettings.AzureSettings.json");

            if (manifestResourceStream == null)
                throw new InvalidOperationException("Unable to find an embedded Azure Settings file");

            return new StreamReader(manifestResourceStream).ReadToEnd();
        }
    }
}