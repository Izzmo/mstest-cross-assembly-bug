using System;
using Gtm.WebService.Models.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebService.Models.Tests.Services.PartnerService
{
   [TestClass]
   public abstract class PartnerServiceBaseTest : ServiceBaseTest<IPartnerService>
   {
      protected static Guid testAuthorizationToken = Guid.Parse("a13a2d87-a1c4-4d16-9013-320f1531ec11");
   }
}
