using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Gtm.WebService.Models;

namespace Gtm.WebService.WebApi.Controllers
{
   /// <summary>
   /// Base controller for which all controllers should inherit.
   /// </summary>
   /// <remarks>Contains useful utility methods.</remarks>
   public class WebApiController : Controller
   {
      /// <summary>
      /// Get the currently authenticated Partner's ID.
      /// </summary>
      /// <returns>Parter GUID</returns>
      protected Guid GetPartnerIdentifier()
      {
         var partnerClaim = User.FindFirst(ClaimTypes.NameIdentifier);

         if (partnerClaim == null || !Guid.TryParse(partnerClaim.Value, out var partnerIdentifier))
            throw new UnauthorizedAccessException("Unable to identify caller.");

         return partnerIdentifier;
      }

      /// <summary>
      /// Get the currently authenticated Partner.
      /// </summary>
      /// <returns>A Partner</returns>
      protected IdentifiablePartner GetPartner()
      {
         var partnerIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier);
         var partnerNameClaim = User.FindFirst(ClaimTypes.Name);
         var partnerAuthTokenClaim = User.FindFirst(ClaimTypes.Authentication);

         if (partnerIdentifierClaim == null || !Guid.TryParse(partnerIdentifierClaim.Value, out var partnerIdentifier))
            throw new UnauthorizedAccessException("Unable to identify caller.");

         if (partnerNameClaim == null)
            throw new UnauthorizedAccessException("Unable to get partner name.");

         if (partnerAuthTokenClaim == null || !Guid.TryParse(partnerAuthTokenClaim.Value, out var partnerAuthToken))
            throw new UnauthorizedAccessException("Unable to get partner authentication token.");

         return new IdentifiablePartner(partnerIdentifier, partnerNameClaim.Value, partnerAuthToken);
      }
   }
}
