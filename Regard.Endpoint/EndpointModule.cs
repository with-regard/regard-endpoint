using Ninject.Modules;

namespace Regard.Endpoint
{
    public class EndpointModule : NinjectModule
    {

        public override void Load()
        {
            Bind<IServiceBusEventProcessorSettings>().To<ConfigurationManagerBasedSettings>().InSingletonScope();
            Bind<IEventProcessor>().To<ServiceBusEventProcessor>().InSingletonScope();
            Bind<IEventValidator>().To<EventValidator>().InSingletonScope();
            Bind<IPayloadValidator>().To<PayloadValidator>().InSingletonScope();
            Bind<IServiceBusClient>().To<ServiceBusClient>().InSingletonScope();
        }
    }
}