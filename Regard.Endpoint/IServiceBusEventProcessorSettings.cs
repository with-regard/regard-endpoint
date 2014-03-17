namespace Regard.Endpoint
{
    internal interface IServiceBusEventProcessorSettings
    {
        string ServiceBusConnectionString { get; }

        string AnalyticsTopicName { get; }
    }
}