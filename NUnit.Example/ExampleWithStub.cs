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
    public class ExampleWithStub
    {
        private static FluentMockServer stub;
        private static string baseUrl;

        [OneTimeSetUp]
        public static void PrepareClass()
        {
            var port = new Random().Next(5000, 6000);
            baseUrl = "http://localhost:" + port;

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
            var bodyContent = new[] {
        pendingTransactions =  new {
          transactionId = "string",
          accountId = "string",
          effectiveDate = "2015-07-20",
          effectiveTime = "15:49:04-07:00",
          postedDate = "2015-07-20T15:49:04-07:00",
          debitOrCredit = "Debit",
          description = "string",
          payment = true,
          paymentMethod = "PayerNotPresent",
          amount = "160.56",
          currency = "GBP",
          balance = "string",
          reference = "string"
        },
        new {
        postedTransactions = new{
          transactionId = "string",
          accountId = "string",
          effectiveDate = "2015-07-20",
          effectiveTime = "string",
          postedDate = "2015-07-20T15:49:04-07:00",
          debitOrCredit = "Debit",
          description = "string",
          payment = true,
          paymentMethod = "PayerNotPresent",
          amount = "string",
          currency = "GBP",
          balance = "string",
          reference = "string"
        },
        },
    };
            stub.Given(
                Request
                .Create()
                    .WithPath("/api/products"))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "aplication/json")
                        .WithBodyAsJson(bodyContent));

            var client = new RestClient(baseUrl);
            var request = new RestRequest("/api/products");

            var response = client.Execute(request);
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.AreEqual(JsonConvert.SerializeObject(bodyContent), response.Content);
        }


    }
}
