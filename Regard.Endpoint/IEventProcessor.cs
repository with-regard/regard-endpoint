using System.Threading.Tasks;

namespace Regard.Endpoint
{
    /// <summary>
    /// Interface implemented by objects that process metrics events
    /// </summary>
    internal interface IEventProcessor
    {
        /// <summary>
        /// Handles an event
        /// </summary>
        Task Process(string organization, string product, string payload);
    }
}
