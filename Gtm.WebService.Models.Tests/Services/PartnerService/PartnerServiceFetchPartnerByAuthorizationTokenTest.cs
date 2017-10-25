using System;
using System.Threading.Tasks;
using Gtm.Common.Data;
using Gtm.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebService.Models.Tests.Services.PartnerService
{
   [TestClass]
   public abstract class PartnerServiceFetchPartnerByAuthorizationTokenTest : PartnerServiceBaseTest
   {
      [TestMethod]
      public async Task ShouldBeAbleToFetchTestPartnerByAuthorizationToken()
      {
         var fetchedPartner = await Service.FetchPartnerByAuthorizationToken(testAuthorizationToken);

         Assert.IsInstanceOfType(fetchedPartner, typeof(IdentifiablePartner));
         Assert.AreEqual(testAuthorizationToken, fetchedPartner.AuthorizationToken);
         Assert.AreEqual("Test", fetchedPartner.Name);
      }

      [TestMethod]
      [ExpectedException(typeof(RecordNotFoundException))]
      public async Task ShouldThrowRecordNotFoundExceptionWhenAuthorizationTokenIsNotFound()
      {
         await Service.FetchPartnerByAuthorizationToken(Guid.NewGuid());
      }

      [TestMethod]
      public void ShouldThrowArgumentExceptionOnInvalidParameters()
      {
         AssertArgumentException.IsThrownForParameter(() => Service.FetchPartnerByAuthorizationToken(Guid.Empty), "partnerAuthorizationToken");
      }
   }
}
