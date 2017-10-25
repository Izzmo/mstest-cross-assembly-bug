using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gtm.WebService.Models.Services;

namespace Gtm.WebService.Models.Tests.Services.DesignService
{
   [TestClass]
   public class FakeDesignServiceTest : DesignServiceBaseTest
   {
      protected override IDesignService CreateService()
      {
         return new FakeDesignService();
      }
   }
}
