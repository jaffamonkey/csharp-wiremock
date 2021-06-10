using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace NUnit.Example
{
    public class UpgradeCheckWithStub
    {
        private static FluentMockServer stub;
        private static string baseUrl;

        [OneTimeSetUp]
        public static void PrepareClass()
        {
            var port = new Random().Next(5000, 6000);
            baseUrl = "http://localhost:" + port;
            Console.WriteLine(baseUrl);
            stub = FluentMockServer.Start(new FluentMockServerSettings
            {
                Urls = new[] { "http://+:" + port },
                ReadStaticMappings = true
            });
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            stub.Stop();
        }

        [Test]
        public void Test()
        {
            var bodyContent =
                new
                {
                    upgradeRequired = true
                };

            stub.Given(
                Request
                .Create()
                    .WithPath("/api/mobile/status/clientinfo"))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "aplication/json")
                        .WithBodyAsJson(bodyContent));

            var client = new RestClient(baseUrl);
            var request = new RestRequest("/api/mobile/status/clientinfo");

            var response = client.Execute(request);
            Console.WriteLine("Your response data is: " + response.Content);
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.AreEqual(JsonConvert.SerializeObject(bodyContent), response.Content);
        }


    }
}
