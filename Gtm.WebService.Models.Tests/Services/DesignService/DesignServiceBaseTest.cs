using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gtm.Common.Data;
using Gtm.TestUtilities;
using Gtm.WebService.Models.Services;

namespace Gtm.WebService.Models.Tests.Services.DesignService
{
   [TestClass]
   public abstract class DesignServiceBaseTest : ServiceBaseTest<IDesignService>
   {
      protected static IdentifiablePartner TestPartner { get; } = new IdentifiablePartner(Guid.Parse("b2d975e3-33f9-43db-9b8c-a2f3256fbe23"), "Test Partner", Guid.Parse("c2d975e3-33f9-43db-9b8c-a2f3256fbe23"));

      [TestMethod]
      public async Task ShouldBeAbleToFetchBlankDesign()
      {
         var designList = await Service.RetrieveDesigns(new List<KeyValuePair<string, string>>(), TestPartner);
         var design = await Service.FetchDesign(designList[0].Identifier);

         Assert.AreEqual(designList[0].Identifier, design.Identifier, "Identifier should be same from retrieve and fetch.");
         Assert.AreEqual(designList[0].Created.By, design.Created.By, "Created.By should be same from retrieve and fetch.");
         Assert.AreEqual(designList[0].Created.On.ToString(), design.Created.On.ToString(), "Created.On should be same from retrieve and fetch.");
         Assert.AreEqual(designList[0].IsEmbellished, design.IsEmbellished, "IsEmbellished should be same from retrieve and fetch.");
         Assert.AreEqual(designList[0].Product.Name, design.Product.Name, "Product should be same from retrieve and fetch.");
      }

      [TestMethod]
      [ExpectedArgumentException(typeof(ArgumentException), "designIdentifier")]
      public async Task ShouldThrowAEWhenUsingEmptyGuid()
      {
         await Service.FetchDesign(Guid.Empty);
      }

      [TestMethod]
      [ExpectedArgumentException(typeof(ArgumentException), "designColorName")]
      public async Task ShouldThrowArgumentExceptionForInvalidColor()
      {
         await Service.RetrieveDesigns(
            new List<KeyValuePair<string, string>>
            {
               new KeyValuePair<string, string>("color", "invalid")
            },
            TestPartner);
      }

      [TestMethod]
      [ExpectedException(typeof(RecordNotFoundException))]
      public async Task ShouldThrowRNFWhenUsingEmptyGuid()
      {
         await Service.FetchDesign(Guid.NewGuid());
      }

      [TestMethod]
      public async Task ShouldAllowRetrieveColors()
      {
         IList<Color> colorList = await Service.RetrieveColors();

         Assert.IsNotNull(colorList);
         Assert.IsTrue(colorList.Count > 0);
      }

      [TestMethod]
      [ExpectedArgumentException(typeof(ArgumentException), "designIdentifier")]
      public async Task ShouldThrowAEWhenUsingEmptyGuidOnFetchProductDetails()
      {
         await Service.FetchProductDetails(Guid.Empty);
      }

      [TestMethod]
      [ExpectedException(typeof(RecordNotFoundException))]
      public async Task ShouldThrowRNFWhenFetchingProductDetailsWithInvalidDesignId()
      {
         await Service.FetchProductDetails(Guid.NewGuid());
      }

      [TestMethod]
      public async Task ShouldAllowFetchProductDetails()
      {
         var designList = await Service.RetrieveDesigns(new List<KeyValuePair<string, string>>(), TestPartner);
         IList<string> productDetails = await Service.FetchProductDetails(designList[0].Identifier);

         Assert.IsNotNull(productDetails);
         Assert.IsTrue(productDetails.Count > 0);
      }

      [TestMethod]
      public async Task ShouldAllowGetColor()
      {
         Color color = await Service.FetchColorByName("Black");

         Assert.IsNotNull(color, "Black should exists");
      }

      [TestMethod]
      [ExpectedArgumentException(typeof(ArgumentException), "designColorName")]
      public async Task ShouldThrowArgumentExceptionWhenFetchingInvalidColor()
      {
         await Service.FetchColorByName("Invalid Color");
      }

      [TestMethod]
      [ExpectedArgumentException(typeof(ArgumentException), "designIdentifier")]
      public async Task ShouldThrowArgumentExceptionWithEmptyDesignIdentifier()
      {
         await Service.FetchSizingMeasurements(Guid.Empty);
      }

      [TestMethod]
      [ExpectedException(typeof(RecordNotFoundException))]
      public async Task ShouldThrowRecordNotFoundExceptionWithNonExistentDesignIdentifier()
      {
         await Service.FetchSizingMeasurements(Guid.NewGuid());
      }
   }
}
