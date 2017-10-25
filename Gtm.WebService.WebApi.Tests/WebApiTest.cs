using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Gtm.WebApi.TestUtilities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Gtm.Common.WebApi;

namespace Gtm.WebService.WebApi.Tests
{
   [TestClass]
   public abstract class WebApiTest<TService, TImplementation> : WebApiTest
      where TService : class
      where TImplementation : class, TService
   {
      protected override void AddServices(IServiceCollection services)
      {
         services.AddSingleton<TService, TImplementation>();
      }
   }

   [TestClass]
   public abstract class WebApiTest : WebTestBase, IDisposable
   {
      private TestServer testServer;

      public WebApiTest()
      {
         Cookies = new CookieContainer();

         testServer = new TestServer(WebHostBuilderFactory.Create(AddServices));
      }

      public override Uri BaseUrl => testServer.BaseAddress;

      public CookieContainer Cookies { get; }

      public void Dispose()
      {
         if (testServer == null)
            return;

         testServer.Dispose();
         testServer = null;
      }

      /// <summary>
      /// Add extra services that your controller depends on.
      /// </summary>
      /// <remarks>
      /// This method is intentionally left empty as the implementation depends on the inherited controller test class.
      /// </remarks>
      /// <param name="services">List of services to be added.</param>
      protected virtual void AddServices(IServiceCollection services)
      {
      }

      protected async Task AssertBadRequestMessage(string resourceUrl, BadRequestMessage expectedResponseBody, object requestBody)
      {
         var actualResponse = await PostJson(resourceUrl, requestBody, HttpStatusCode.BadRequest, WebApiTestHeaders.Headers);

         AssertJson.Matches(expectedResponseBody, actualResponse);
      }

      protected override async Task<JsonValue> GetJson(string relativeUrl, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null)
      {
         var response = await Get(relativeUrl, headers);

         await CaptureDetailsIfInternalServerError(response);

         Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

         return new JsonValue(await response.Content.ReadAsStringAsync());
      }

      protected override Task<HttpResponseMessage> Get(string relativeUrl, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null)
      {
         var request = CreateRequest(relativeUrl, headers);

         return request.GetAsync();
      }

      protected override async Task<HttpResponseMessage> PostJson(string relativeUrl, string body, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null)
      {
         var request = CreateRequest(relativeUrl, headers);

         request.And(message =>
         {
            message.Content = new StringContent(body, Encoding.UTF8, "application/json");
         });

         var response = await request.PostAsync();

         await CaptureDetailsIfInternalServerError(response);

         return response;
      }

      protected async Task<HttpResponseMessage> PatchJson(string relativeUrl, object body, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null)
      {
         var request = CreateRequest(relativeUrl, headers);
         var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);

         request.And(message =>
         {
            message.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
         });

         var response = await request.SendAsync("patch");

         await CaptureDetailsIfInternalServerError(response);

         return response;
      }

      private static async Task CaptureDetailsIfInternalServerError(HttpResponseMessage response)
      {
         if (response.StatusCode == HttpStatusCode.InternalServerError)
         {
            string locationMessage;

            try
            {
               var tempFileName = Path.GetTempFileName() + ".html";

               using (var file = new StreamWriter(tempFileName))
                  file.Write(await response.Content.ReadAsStringAsync());

               locationMessage = $"More information located at {tempFileName}.";
            }
            catch
            {
               locationMessage = "Failed to save server response to file.";
            }

            Assert.Fail($"An internal server error occurred. {locationMessage}");
         }
      }

      private static void AddHeaders(RequestBuilder request, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers)
      {
         request.AddHeader("Content-Type", "application/json");
         request.AddHeader("Accept", "application/json; charset=utf-8");

         if (headers == null)
            return;

         foreach (var header in headers)
            request.AddHeader(header.Key.ToString(), header.Value);
      }

      private RequestBuilder CreateRequest(string relativeUrl, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers)
      {
         var absoluteUrl = new Uri(testServer.BaseAddress, relativeUrl);
         var request = testServer.CreateRequest(absoluteUrl.ToString());

         AddHeaders(request, headers);

         return request;
      }
   }
}
