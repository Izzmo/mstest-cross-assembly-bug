using System;
using System.Threading.Tasks;
using Gtm.WebService.Models.Services;
using Gtm.Common.Data;
using Gtm.Common;

namespace Gtm.WebService.Models.Tests.Services.PartnerService
{
   public class FakePartnerService : IPartnerService
   {
      private static IdentifiablePartner TestPartner => new IdentifiablePartner(Guid.Parse("b2d975e3-33f9-43db-9b8c-a2f3256fbe23"), "Test", Guid.Parse("a13a2d87-a1c4-4d16-9013-320f1531ec11"));

      public Task<IdentifiablePartner> FetchPartnerByAuthorizationToken(Guid partnerAuthorizationToken)
      {
         ParameterVerifier.VerifyIsNotEmpty(partnerAuthorizationToken, nameof(partnerAuthorizationToken));

         if (partnerAuthorizationToken == TestPartner.AuthorizationToken)
            return Task.FromResult(TestPartner);

         throw new RecordNotFoundException("Partner Token", partnerAuthorizationToken.ToString("N"));
      }
   }
}
