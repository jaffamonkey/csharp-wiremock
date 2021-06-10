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
    public class CardSummaryWithStub
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
                    accountId = "11111",
                    customerId = "11111",
                    creditLimit = "1750",
                    currentBalance = "345.55",
                    availableCredit = "123.00",
                    amountDue = "80",
                    nextPaymentDueDate = "2015-07-20",
                    nextPaymentDueAmount = "string",
                    paymentMethod = "DirectDebit",
                    paymentUrgency = "Soon",
                    cpaStatus = "Valid",
                    status = "Open",
                    paymentsMissed = 0
                };

            stub.Given(
                Request
                .Create()
                    .WithPath("/api/cards/{cardId}/summary"))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "aplication/json")
                        .WithBodyAsJson(bodyContent));

            var client = new RestClient(baseUrl);
            var request = new RestRequest("/api/cards/{cardId}/summary");

            var response = client.Execute(request);
            Console.WriteLine("Your response data is: " + response.Content);
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.AreEqual(JsonConvert.SerializeObject(bodyContent), response.Content);
        }


    }
}
