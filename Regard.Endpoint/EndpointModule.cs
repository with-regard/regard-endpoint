using Ninject.Modules;

namespace Regard.Endpoint
{
    internal class EndpointModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceBusEventProcessorSettings>().To<ServiceBusEventProcessorSettings>().InSingletonScope();
            Bind<IEventProcessor>().To<ServiceBusEventProcessor>().InSingletonScope();
        }
    }
}