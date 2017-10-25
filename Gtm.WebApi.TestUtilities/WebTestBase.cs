using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gtm.Common;

namespace Gtm.WebApi.TestUtilities
{
   /// <summary>
   /// Provides an interface for convenient testing of web endpoints.
   /// </summary>
   public abstract class WebTestBase
   {
      private readonly Dictionary<HttpRequestHeader, string> headers;

      /// <summary>
      /// Initializes a new instance of the <see cref="WebTestBase"/> class with the given <paramref name="baseUrl"/> and <paramref name="headers"/>.
      /// </summary>
      /// <param name="baseUrl">The root URL to be used for all requests.</param>
      /// <param name="headers">The headers to be used for all requests.</param>
      /// <exception cref="ArgumentException">
      /// Thrown if <paramref name="baseUrl"/> is not an absolute URI.  That is, <c><paramref name="baseUrl"/>.<see cref="Uri.IsAbsoluteUri"/> == false</c>.
      /// </exception>
      protected WebTestBase(Uri baseUrl = null, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null)
      {
         ParameterVerifier.VerifyIsTrue(baseUrl == null || baseUrl.IsAbsoluteUri, nameof(baseUrl), "Base URL must be an absolute URI.");

         BaseUrl = baseUrl;

         this.headers = new Dictionary<HttpRequestHeader, string>();

         if (headers == null)
            return;

         foreach (var h in headers)
            this.headers.Add(h.Key, h.Value);
      }

      /// <summary>
      /// Gets the URL that will serve as the root for all requests.
      /// </summary>
      public virtual Uri BaseUrl { get; }

      /// <summary>
      /// Gets the headers to be used for all requests.
      /// </summary>
      public IReadOnlyDictionary<HttpRequestHeader, string> Headers => headers;

      /// <summary>
      /// Generate and return a new unique identifier.
      /// </summary>
      /// <returns>
      /// A newly generated identifier.
      /// </returns>
      public static string NewId() => Guid.NewGuid().ToString("N");

      /// <summary>
      /// Appends the given <paramref name="relativeUrl"/> to the <see cref="BaseUrl"/>.
      /// </summary>
      /// <param name="relativeUrl">Url to append.</param>
      /// <returns>
      /// A new <see cref="Uri"/> with the <see cref="BaseUrl"/> and <paramref name="relativeUrl"/> combined.
      /// </returns>
      protected Uri AppendUrl(string relativeUrl) => new Uri(BaseUrl?.AbsoluteUri + relativeUrl);

      /// <summary>
      /// Executes a GET request and returns the response as a <see cref="HttpResponseMessage"/> instance.
      /// </summary>
      /// <param name="relativeUrl">The URL to be appended to <see cref="BaseUrl"/>.</param>
      /// <param name="additionalHeaders">Headers to be used in addition to <see cref="Headers"/>.</param>
      /// <returns>
      /// The response.
      /// </returns>
      protected abstract Task<HttpResponseMessage> Get(string relativeUrl, IEnumerable<KeyValuePair<HttpRequestHeader, string>> additionalHeaders = null);

      /// <summary>
      /// Executes a GET request and returns the response (expected as JSON) as a <see cref="JsonValue"/> instance.
      /// </summary>
      /// <param name="relativeUrl">The URL to be appended to <see cref="BaseUrl"/>.</param>
      /// <param name="additionalHeaders">Headers to be used in addition to <see cref="Headers"/>.</param>
      /// <returns>
      /// The response in JSON.
      /// </returns>
      /// <exception cref="AssertFailedException">
      /// Thrown if resulting <see cref="HttpResponseMessage.StatusCode"/> is not <see cref="HttpStatusCode.OK"/>.
      /// </exception>
      protected virtual Task<JsonValue> GetJson(string relativeUrl, IEnumerable<KeyValuePair<HttpRequestHeader, string>> additionalHeaders = null)
      {
         return GetJson(relativeUrl, HttpStatusCode.OK, additionalHeaders);
      }

      /// <summary>
      /// Executes a GET request and returns the response (expected as JSON) as a <see cref="JsonValue"/> instance.
      /// </summary>
      /// <param name="relativeUrl">The URL to be appended to <see cref="BaseUrl"/>.</param>
      /// <param name="expectedStatusCode">The resulting code expected.</param>
      /// <param name="additionalHeaders">Headers to be used in addition to <see cref="Headers"/>.</param>
      /// <returns>
      /// The response in JSON.
      /// </returns>
      /// <exception cref="AssertFailedException">
      /// Thrown if resulting <see cref="HttpResponseMessage.StatusCode"/> is not <see cref="HttpStatusCode.OK"/>.
      /// </exception>
      protected virtual async Task<JsonValue> GetJson(string relativeUrl, HttpStatusCode expectedStatusCode, IEnumerable<KeyValuePair<HttpRequestHeader, string>> additionalHeaders = null)
      {
         var message = await Get(relativeUrl, additionalHeaders);

         Assert.AreEqual(expectedStatusCode, message.StatusCode, $"A status code of {expectedStatusCode} was expected, but {message.StatusCode} was returned.");

         return new JsonValue(await message.Content.ReadAsStringAsync());
      }

      /// <summary>
      /// Executes a POST request and returns the response.
      /// </summary>
      /// <param name="relativeUrl">The URL to be appended to <see cref="BaseUrl"/>.</param>
      /// <param name="body">The payload for the request as text.</param>
      /// <param name="additionalHeaders">Headers to be used in addition to <see cref="Headers"/>.</param>
      /// <returns>
      /// The response.
      /// </returns>
      protected abstract Task<HttpResponseMessage> PostJson(string relativeUrl, string body, IEnumerable<KeyValuePair<HttpRequestHeader, string>> additionalHeaders = null);

      /// <summary>
      /// Executes a POST request and returns the response.
      /// </summary>
      /// <param name="relativeUrl">The URL to be appended to <see cref="BaseUrl"/>.</param>
      /// <param name="body">The payload for the request as an object, convenient for anonymous types.</param>
      /// <param name="additionalHeaders">Headers to be used in addition to <see cref="Headers"/>.</param>
      /// <returns>
      /// The response.
      /// </returns>
      protected virtual Task<HttpResponseMessage> PostJson(string relativeUrl, object body, IEnumerable<KeyValuePair<HttpRequestHeader, string>> additionalHeaders = null)
      {
         return PostJson(relativeUrl, new JsonValue(body), additionalHeaders);
      }

      /// <summary>
      /// Executes a POST request and returns the response.
      /// </summary>
      /// <param name="relativeUrl">The URL to be appended to <see cref="BaseUrl"/>.</param>
      /// <param name="body">The payload for the request as text.</param>
      /// <param name="expectedStatusCode">The resulting code expected.</param>
      /// <param name="additionalHeaders">Headers to be used in addition to <see cref="Headers"/>.</param>
      /// <returns>
      /// The response.
      /// </returns>
      /// <exception cref="AssertFailedException">
      /// Thrown if resulting <see cref="HttpResponseMessage.StatusCode"/> is not <paramref name="expectedStatusCode"/>.
      /// </exception>
      protected virtual async Task<JsonValue> PostJson(string relativeUrl, string body, HttpStatusCode expectedStatusCode, IEnumerable<KeyValuePair<HttpRequestHeader, string>> additionalHeaders = null)
      {
         var message = await PostJson(relativeUrl, body, additionalHeaders);

         Assert.AreEqual(expectedStatusCode, message.StatusCode, $"A status code of {expectedStatusCode} was expected, but {message.StatusCode} was returned.");

         return new JsonValue(await message.Content.ReadAsStringAsync());
      }

      /// <summary>
      /// Executes a POST request and returns the response.
      /// </summary>
      /// <param name="relativeUrl">The URL to be appended to <see cref="BaseUrl"/>.</param>
      /// <param name="body">The payload for the request as an object, convenient for anonymous types.</param>
      /// <param name="expectedStatusCode">The resulting code expected.</param>
      /// <param name="additionalHeaders">Headers to be used in addition to <see cref="Headers"/>.</param>
      /// <returns>
      /// The response.
      /// </returns>
      /// <exception cref="AssertFailedException">
      /// Thrown if resulting <see cref="HttpResponseMessage.StatusCode"/> is not <paramref name="expectedStatusCode"/>.
      /// </exception>
      protected virtual Task<JsonValue> PostJson(string relativeUrl, object body, HttpStatusCode expectedStatusCode, IEnumerable<KeyValuePair<HttpRequestHeader, string>> additionalHeaders = null)
      {
         return PostJson(relativeUrl, new JsonValue(body), expectedStatusCode, additionalHeaders);
      }
   }
}
