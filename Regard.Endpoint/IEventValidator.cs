using Newtonsoft.Json.Linq;

namespace Regard.Endpoint
{
    internal interface IEventValidator
    {
        bool IsValid(string individualEvent);

        bool IsValid(JObject eventPayload);
    }
}