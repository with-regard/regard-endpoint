using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Regard.Endpoint.Tests
{
    [TestFixture]
    public class JsonValidatorTests
    {
        [Test]
        public void EmptyObjectIsInvalid()
        {
            Validate(new JObject()).Should().BeFalse();
        }

        [Test]
        public void ObjectWithRequiredPropertiesIsValid()
        {
            Validate(new JObject
                     {
                         {"session-id", "some-string"},
                         {"event-type", "some-other-string"},
                         {"user-id", "some-user-id"},
                         {"time", 1234}
                     }).Should().BeTrue();
        }

        [Test]
        public void ObjectWithRequiredPropertiesButWrongTypeIsInvalid()
        {
            Validate(new JObject
                     {
                         {"session-id", "some-string"},
                         {"event-type", "some-other-string"},
                         {"user-id", "some-user-id"},
                         {"time", "1234"}
                     }).Should().BeFalse();
        }

        [Test]
        public void ObjectWithInvalidJsonIsAnInvalidEvent()
        {
            Validate("invalid json").Should().BeFalse();
        }

        private bool Validate(JObject obj)
        {
            return Validate(JsonConvert.SerializeObject(obj));
        }

        private bool Validate(string json)
        {
            return new EventValidator().IsValid(JObject.Parse(json));
        }
    }
}
