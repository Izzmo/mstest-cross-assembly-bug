using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Gtm.WebService.Models.Services;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// Middleware Service for API Key Authentication
   /// </summary>
   public class ApiKeyAuthenticationMiddleware : AuthenticationMiddleware<ApiKeyAuthenticationOptions>
   {
      private readonly IPartnerService partnerService;

      /// <summary>
      /// Initializes a new instance of the <see cref="ApiKeyAuthenticationMiddleware"/> class.
      /// </summary>
      /// <param name="next">The next middleware class.</param>
      /// <param name="options">ApiKeyAuthenticationOptions</param>
      /// <param name="loggerFactory">The logger factory class.</param>
      /// <param name="encoder">The URL Encoder.</param>
      /// <param name="partnerService">The partner microservice proxy.</param>
      public ApiKeyAuthenticationMiddleware(
         RequestDelegate next,
         IOptions<ApiKeyAuthenticationOptions> options,
         ILoggerFactory loggerFactory,
         UrlEncoder encoder,
         IPartnerService partnerService)
         : base(next, options, loggerFactory, encoder)
      {
         this.partnerService = partnerService;
      }

      /// <summary>
      /// Create an authentication handler.
      /// </summary>
      /// <returns>An authentication handler.</returns>
      protected override AuthenticationHandler<ApiKeyAuthenticationOptions> CreateHandler()
      {
         return new ApiKeyAuthenticationHandler(partnerService);
      }
   }
}
