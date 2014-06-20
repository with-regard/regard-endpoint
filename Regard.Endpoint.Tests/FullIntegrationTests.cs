using System.Net.Http;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Regard.Endpoint.Tests
{
    internal class FullIntegrationTests
    {
        [Test]
        public void SingleEventTest()
        {
            PostEvents(JsonConvert.SerializeObject(GetSampleEvent())).Should().Be(1);
        }

        [Test]
        public void MultipleEventsTest()
        {
            var jArray = new JArray(GetSampleEvent(), GetSampleEvent(), GetSampleEvent());

            PostEvents(JsonConvert.SerializeObject(jArray)).Should().Be(3);
        }

        private int PostEvents(string events)
        {
            var nullServiceBusClient = new NullServiceBusClient();

            using (var testServer = GetTestServer(nullServiceBusClient))
            {
                var httpResponseMessage = testServer.HttpClient.PostAsync("/track/v1/regard/regardtest/event", new StringContent(events)).Result;

                httpResponseMessage.IsSuccessStatusCode.Should().BeTrue();

                return nullServiceBusClient.PostedEvents;
            }
        }

        private static JObject GetSampleEvent()
        {
            return new JObject
                   {
                       {"session-id", "some-string"},
                       {"event-type", "some-other-string"},
                       {"user-id", "some-user-id"},
                       {"time", 1234}
                   };
        }

        private static TestServer GetTestServer(NullServiceBusClient client)
        {
            return TestServer.Create(new Startup(new TestModule(client)).Configuration);
        }
    }
}
