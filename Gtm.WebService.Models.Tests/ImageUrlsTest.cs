using System;
using Gtm.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebService.Models.Tests
{
   [TestClass]
   public class ImageUrlsTest
   {
      private Uri testUri = new Uri("http://localhost");

      [TestMethod]
      public void ShouldSerializeDataContract()
      {
         var instanceForSerialization = new ImageUrls(testUri, testUri, testUri, testUri);
         var deserializedInstance = DataContractSerialization.GetDeserializedInstance(instanceForSerialization);

         Assert.AreEqual(instanceForSerialization.Front, deserializedInstance.Front, "Front image should be equal.");
         Assert.AreEqual(instanceForSerialization.Left, deserializedInstance.Left, "Left image should be equal.");
         Assert.AreEqual(instanceForSerialization.Right, deserializedInstance.Right, "Right image should be equal.");
         Assert.AreEqual(instanceForSerialization.Back, deserializedInstance.Back, "Back image should be equal.");
      }
   }
}
