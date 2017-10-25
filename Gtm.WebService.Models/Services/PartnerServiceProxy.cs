using System;
using System.Threading.Tasks;

namespace Gtm.WebService.Models.Services
{
   /// <summary>
   /// Proxy class for <see cref="IPartnerService"/>
   /// </summary>
   public class PartnerServiceProxy : StatefulServiceProxy<IPartnerService>, IPartnerService
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="PartnerServiceProxy"/> class.
      /// </summary>
      public PartnerServiceProxy()
         : base(new Uri("fabric:/Gtm.WebService/PartnerService"))
      {
      }

      /// <summary>
      /// See <see cref="IPartnerService"/>
      /// </summary>
      public Task<IdentifiablePartner> FetchPartnerByAuthorizationToken(Guid partnerAuthorizationToken)
      {
         return InvokeService(() => Service.FetchPartnerByAuthorizationToken(partnerAuthorizationToken));
      }
   }
}
