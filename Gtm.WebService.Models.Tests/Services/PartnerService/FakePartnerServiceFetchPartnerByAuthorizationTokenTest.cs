using Gtm.WebService.Models.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebService.Models.Tests.Services.PartnerService
{
   [TestClass]
   public class FakePartnerServiceFetchPartnerByAuthorizationTokenTest : PartnerServiceFetchPartnerByAuthorizationTokenTest
   {
      protected override IPartnerService CreateService()
      {
         return new FakePartnerService();
      }
   }
}
