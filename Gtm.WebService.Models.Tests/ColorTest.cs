using Gtm.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebService.Models.Tests
{
   [TestClass]
   public class ColorTest
   {
      private const string TestColorName = "Test Color";
      private const string TestHexCode = "BDBDBD";

      [TestMethod]
      public void ShouldThrowExceptionsOnConstructorParameters()
      {
         AssertArgumentNullException.IsThrownForParameter(() => new Color(null, TestHexCode), "name");
         AssertArgumentNullException.IsThrownForParameter(() => new Color(TestColorName, null), "hexCode");

         AssertArgumentException.IsThrownForParameter(() => new Color(string.Empty, TestHexCode), "name");
         AssertArgumentException.IsThrownForParameter(() => new Color(TestColorName, string.Empty), "hexCode");
      }

      [TestMethod]
      public void ShouldAllowAccessToProperties()
      {
         var expectedColor = new Color(TestColorName, TestHexCode);

         Assert.AreEqual(TestColorName, expectedColor.Name, "Color Name should be equal.");
         Assert.AreEqual(TestHexCode, expectedColor.HexCode, "Color Hex Code should be equal.");
      }

      [TestMethod]
      public void ShouldAllowSerialization()
      {
         var instanceForSerialization = new Color(TestColorName, TestHexCode);
         var deserializedInstance = DataContractSerialization.GetDeserializedInstance(instanceForSerialization);

         Assert.AreEqual(instanceForSerialization.Name, deserializedInstance.Name, "Name should be equal.");
         Assert.AreEqual(instanceForSerialization.HexCode, deserializedInstance.HexCode, "Hex Code should be equal.");
      }

      [TestMethod]
      public void ShouldBeAbleToClone()
      {
         var color = new Color(TestColorName, TestHexCode);

         var cloned = color.Clone();

         Assert.AreEqual(color.Name, cloned.Name, "Name should be equal.");
         Assert.AreEqual(color.HexCode, cloned.HexCode, "HexCode should be equal.");
      }
   }
}
