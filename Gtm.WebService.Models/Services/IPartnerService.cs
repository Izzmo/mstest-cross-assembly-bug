using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Gtm.Common.Data;

namespace Gtm.WebService.Models.Services
{
   /// <summary>
   /// The interface for the PartnerService.
   /// </summary>
   public interface IPartnerService : IService
   {
      /// <summary>
      /// Fetches a partner from the system using the validation token.
      /// </summary>
      /// <param name="partnerAuthorizationToken">The validation token of the partner to fetch.</param>
      /// <returns>An identifiable partner.</returns>
      /// <exception cref="ArgumentException">Thrown if <paramref name="partnerAuthorizationToken"/> is <see cref="Guid.Empty"/>.</exception>
      /// <exception cref="RecordNotFoundException">Thrown if <paramref name="partnerAuthorizationToken"/> is not found.</exception>
      Task<IdentifiablePartner> FetchPartnerByAuthorizationToken(Guid partnerAuthorizationToken);
   }
}
