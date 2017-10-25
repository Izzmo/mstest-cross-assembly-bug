using System.Collections.Generic;
using System.Net;

namespace Gtm.WebService.WebApi.Tests
{
   /// <summary>
   /// Convenient test values for headers.
   /// </summary>
   public static class WebApiTestHeaders
   {
      /// <summary>
      /// The authentication token for the test user.
      /// </summary>
      public const string TestAuthorizationToken = "a13a2d87a1c44d169013320f1531ec11";

      /// <summary>
      /// The name of the test partner to which the <see cref="TestAuthorizationToken"/> belongs.
      /// </summary>
      /// <remarks>
      /// This value is helpful for test assertions when validating
      /// <see cref="VersionInfo"/>.<see cref="VersionInfo.By"/>, for example.
      /// </remarks>
      public const string TestPartnerName = "Test";

      /// <summary>
      /// Common headers to be used for convenient use with methods on <see cref="WebApiTest"/>.
      /// </summary>
      public static IReadOnlyDictionary<HttpRequestHeader, string> Headers = new Dictionary<HttpRequestHeader, string>
      {
         {
            HttpRequestHeader.Authorization,
            "Bearer " + TestAuthorizationToken
         },
         {
            HttpRequestHeader.Accept,
            "application/json;profile=\"https://en.wikipedia.org/wiki/PascalCase\""
         }
      };
   }
}
