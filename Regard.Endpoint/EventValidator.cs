using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Regard.Endpoint
{
    internal class EventValidator : IEventValidator
    {
        private readonly JsonSchema m_EventSchema;

        public EventValidator()
        {
            m_EventSchema = JsonSchema.Parse(@"
                        {    
                            'title': 'regard-event',
                            'type': 'object',
                            'additionalProperties': false,
                            'properties': {
                                'session-id': {
                                    'description': '',
                                    'required': true,
                                    'type': 'string'
                                },
                                'user-id': {
                                    'description': '',
                                    'required': true,
                                    'type': 'string'
                                },
                                'event-type': {
                                    'description': '',
                                    'required': true,
                                    'type': 'string'
                                },
                                'time': {
                                    'description': 'the time the event started, or happened. Sent as unix epoch time',
                                    'required': true,
                                    'type': 'integer'
                                },
                                'data': {
                                    'description': '',
                                    'required': false,
                                    'type': 'object'
                                }
                            }
                        }"
                );
        }

        public bool IsValid(string individualEvent)
        {
            try
            {
                return IsValid(JObject.Parse(individualEvent));
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        public bool IsValid(JObject individualEventObject)
        {
            try
            {
                return individualEventObject.IsValid(m_EventSchema);
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
    }
}