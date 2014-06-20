using Newtonsoft.Json.Linq;

namespace Regard.Endpoint
{
    internal interface IEventValidator
    {
        bool IsValid(JObject eventPayload);
    }
}