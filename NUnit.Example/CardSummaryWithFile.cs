using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using WireMock.Server;
using WireMock.Settings;

namespace NUnit.Example
{
    public class CardSummaryWithFile
    {

        private static FluentMockServer stub;
        private static string baseUrl;

        [OneTimeSetUp]
        public static void PrepareClass()
        {
            var port = new Random().Next(5000, 6000);
            baseUrl = "http://localhost:" + port;
            // baseUrl = "https://apim-plat-neu-dev1-api.azure-api.net/vanquiscardscardsummary/";

            stub = FluentMockServer.Start(new FluentMockServerSettings
            {
                Urls = new[] { "http://+:" + port },
                ReadStaticMappings = true
            }); ;
        }

        [OneTimeTearDown]
        public static void CleanClass()
        {
            // stub.Stop();
        }


        [Test]
        public void Test()
        {
            var bodyContent = new
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

            var client = new RestClient(baseUrl);
            var request = new RestRequest("/api/cards/0001134000028324397/summary");

            var response = client.Execute(request);
            Console.WriteLine("Your response data is: " + response.Content);
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.AreEqual(JsonConvert.SerializeObject(bodyContent), response.Content);
        }
    }
}
