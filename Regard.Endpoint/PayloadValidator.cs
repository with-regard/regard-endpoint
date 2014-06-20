using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Regard.Endpoint
{
    internal class PayloadValidator : IPayloadValidator
    {
        private readonly IEventValidator m_Validator;

        public PayloadValidator(IEventValidator validator)
        {
            m_Validator = validator;
        }

        public bool TryGetEvents(string payload, out IEnumerable<string> validatedEvents)
        {
            try
            {
                JObject[] flattenEvents;
                if (!TryFlattenEvents(payload, out flattenEvents))
                {
                    validatedEvents = null;
                    return false;
                }

                validatedEvents = flattenEvents.Select(o => o.ToString()).ToList();
                return flattenEvents.All(s => m_Validator.IsValid(s));
            }
            catch (JsonReaderException)
            {
                validatedEvents = null;
                return false;
            }
        }

        private bool TryFlattenEvents(string payload, out JObject[] events)
        {            
            var deserializeObject = JsonConvert.DeserializeObject(payload);

            if (deserializeObject is JArray)
            {
                events = ((JArray)deserializeObject)
                    .Children()
                    .Select(x => x.ToObject<JObject>())
                    .ToArray();
                return true;
            }
            else if (deserializeObject is JObject)
            {
                events = new[] { (JObject)deserializeObject };
                return true;
            }

            events = null;
            return false;
        }
    }
}