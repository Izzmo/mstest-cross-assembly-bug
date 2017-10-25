using Microsoft.AspNetCore.Builder;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// API Key Authentication Options
   /// </summary>
   public class ApiKeyAuthenticationOptions : AuthenticationOptions
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ApiKeyAuthenticationOptions"/> class.
      /// </summary>
      public ApiKeyAuthenticationOptions()
         : base()
      {
         AuthenticationScheme = "Bearer";
         AutomaticAuthenticate = true;
         AutomaticChallenge = true;
      }
   }
}
