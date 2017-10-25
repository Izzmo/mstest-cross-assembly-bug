using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gtm.WebApi.TestUtilities;
using Gtm.WebService.Models.Services;
using Gtm.WebService.Models.Tests.Services.DesignService;

namespace Gtm.WebService.WebApi.Tests.Controllers
{
   [TestClass]
   public class ColorControllerTest : ControllerTest<IDesignService, FakeDesignService>
   {
      [TestMethod]
      public async Task ShouldRequireAuthentication()
      {
         var response = await Get("/color");

         Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
      }

      [TestMethod]
      public async Task ShouldReturnColorListOnGetColor()
      {
         var expectedColorList = new[]
         {
            new
            {
               Name = "Black",
               HexCode = "000000"
            },
            new
            {
               Name = "Silver",
               HexCode = "898a8e"
            },
            new
            {
               Name = "Orange",
               HexCode = "e15622"
            }
         };

         var actual = await GetJson("/color", WebApiTestHeaders.Headers);

         AssertJson.Matches(expectedColorList, actual);
      }
   }
}
