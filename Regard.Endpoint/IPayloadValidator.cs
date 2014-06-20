using System.Collections.Generic;

namespace Regard.Endpoint
{
    internal interface IPayloadValidator
    {
        bool TryGetEvents(string payload, out IEnumerable<string> validatedEvents);
    }
}