using System.Threading.Tasks;

namespace Regard.Endpoint
{
    internal interface IServiceBusClient
    {
        Task Post(string message);
    }
}