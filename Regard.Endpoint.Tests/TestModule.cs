namespace Regard.Endpoint.Tests
{
    internal class TestModule : EndpointModule
    {
        private readonly NullServiceBusClient m_NullServiceBusClient;

        public TestModule(NullServiceBusClient nullServiceBusClient)
        {
            m_NullServiceBusClient = nullServiceBusClient;
        }

        public override void Load()
        {
            base.Load();

            Rebind<IServiceBusClient>().ToConstant(m_NullServiceBusClient);
        }
    }
}