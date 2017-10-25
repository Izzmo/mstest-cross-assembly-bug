using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gtm.TestUtilities;

namespace Gtm.WebService.Models.Tests
{
   [TestClass]
   public class SizeTest
   {
      private const string DefaultName = "M";
      private const decimal DefaultPrice = 5m;

      [TestMethod]
      public void ShouldThrowArgumentExceptionOnInvalidParametersInConstructor()
      {
         AssertArgumentException.IsThrownForParameter(() => new Size(string.Empty, DefaultPrice), "name");

         AssertArgumentNullException.IsThrownForParameter(() => new Size(null, DefaultPrice), "name");

         AssertException<ArgumentOutOfRangeException>.IsThrownForParameter(() => new Size(DefaultName, 0m), "price");
      }

      [TestMethod]
      public void ShouldBeAbleToAccessProperties()
      {
         var size = new Size(DefaultName, DefaultPrice);

         Assert.AreEqual(DefaultName, size.Name, "Name should be equal from constructor.");
         Assert.AreEqual(DefaultPrice, size.Price, "Price should be equal from constructor.");
      }

      [TestMethod]
      public void ShouldSerializeDataContract()
      {
         var instanceForSerialization = new Size(DefaultName, DefaultPrice);
         var deserializedInstance = DataContractSerialization.GetDeserializedInstance(instanceForSerialization);

         Assert.AreEqual(instanceForSerialization.Name, deserializedInstance.Name, "Name should be equal.");
         Assert.AreEqual(instanceForSerialization.Price, deserializedInstance.Price, "Price should be equal.");
      }

      [TestMethod]
      public void ShouldBeAbleToClone()
      {
         var size = new Size(DefaultName, DefaultPrice);

         var cloned = size.Clone();

         Assert.AreEqual(size.Name, cloned.Name, "Name should be equal.");
         Assert.AreEqual(size.Price, cloned.Price, "Price should be equal.");
      }
   }
}
