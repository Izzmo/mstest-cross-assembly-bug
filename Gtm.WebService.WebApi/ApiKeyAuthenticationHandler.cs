using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Gtm.Common;
using Gtm.WebService.Models.Services;
using Gtm.WebService.Models;
using Gtm.Common.Data;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// Authentication Handler for an API Key
   /// </summary>
   public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
   {
      private readonly IPartnerService partnerService;

      /// <summary>
      /// Initializes a new instance of the <see cref="ApiKeyAuthenticationHandler"/> class.
      /// </summary>
      /// <param name="partnerService">The partner microservice proxy.</param>
      public ApiKeyAuthenticationHandler(IPartnerService partnerService)
      {
         this.partnerService = partnerService;
      }

      /// <summary>
      /// Authentication Handler.
      /// </summary>
      /// <returns>AuthenticateResult</returns>
      protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
      {
         BearerAuthorizationHeader header;
         IdentifiablePartner partner;

         try
         {
            header = new BearerAuthorizationHeader(Request.Headers["Authorization"]);
            partner = await partnerService.FetchPartnerByAuthorizationToken(header.AuthorizationToken);
         }
         catch (ArgumentNullException)
         {
            return AuthenticateResult.Skip();
         }
         catch (ArgumentException)
         {
            return AuthenticateResult.Fail("Authorization token is invalid.");
         }
         catch (RecordNotFoundException)
         {
            return AuthenticateResult.Fail("Authorization token is invalid.");
         }

         var claimsIdentity = new ClaimsIdentity("ApiKey");
         claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, partner.Name));
         claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, partner.Identifier.ToString("N")));
         claimsIdentity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, "ApiKey"));
         claimsIdentity.AddClaim(new Claim(ClaimTypes.Authentication, header.AuthorizationToken.ToString("N")));

         var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
         return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), Options.AuthenticationScheme));
      }

      /// <summary>
      /// Wrapper for an Authorization Web Request Header
      /// </summary>
      private class BearerAuthorizationHeader
      {
         /// <summary>
         /// Initializes a new instance of the <see cref="BearerAuthorizationHeader"/> class.
         /// </summary>
         /// <param name="authorizationHeaderValue">Authorization header value.</param>
         /// <exception cref="ArgumentNullException">Thrown if <paramref name="authorizationHeaderValue"/> is <c>null</c>.</exception>
         /// <exception cref="ArgumentException">Thrown if <paramref name="authorizationHeaderValue"/> is invalid.</exception>
         public BearerAuthorizationHeader(string authorizationHeaderValue)
         {
            ParameterVerifier.VerifyIsNotNullOrEmpty(authorizationHeaderValue, nameof(authorizationHeaderValue));

            var match = Regex.Match(authorizationHeaderValue, @"Bearer (.+)", RegexOptions.IgnoreCase);

            if (!match.Success || !Guid.TryParse(match.Groups[1].Value, out var value))
               throw new ArgumentException("Authorization header invalid.", nameof(authorizationHeaderValue));

            AuthorizationToken = value;
         }

         /// <summary>
         /// Gets the authorization token passed in the header.
         /// </summary>
         public Guid AuthorizationToken { get; }
      }
   }
}
