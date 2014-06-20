using System.Threading;
using System.Threading.Tasks;

namespace Regard.Endpoint.Tests
{
    internal class NullServiceBusClient : IServiceBusClient
    {
        private int m_PostedEvents;

        public Task Post(string message)
        {
            Interlocked.Increment(ref m_PostedEvents);

            var taskCompletionSource = new TaskCompletionSource<object>();
            taskCompletionSource.SetResult(null);
            return taskCompletionSource.Task;
        }

        public int PostedEvents
        {
            get { return m_PostedEvents; }
        }
    }
}